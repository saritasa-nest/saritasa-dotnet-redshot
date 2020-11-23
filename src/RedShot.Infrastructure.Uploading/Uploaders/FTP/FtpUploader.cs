using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP/FTPS uploader.
    /// </summary>
    internal sealed class FtpUploader : BaseFtpUploader, IDisposable, IAsyncDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly FtpAccount account;
        private FtpClient client;
        private bool disposed;
        private bool isNeedToHandle;

        /// <summary>
        /// Initializes FTP/FTPS uploader.
        /// </summary>
        public FtpUploader(FtpAccount account)
        {
            this.account = account;
            isNeedToHandle = true;
            InitializeFtpClient();
        }

        private void InitializeFtpClient()
        {
            client = new FtpClient()
            {
                Host = account.Host,
                Port = account.Port,
                Credentials = new NetworkCredential(account.Username, account.Password)
            };

            if (account.IsActive)
            {
                client.DataConnectionType = FtpDataConnectionType.AutoActive;
            }
            else
            {
                client.DataConnectionType = FtpDataConnectionType.AutoPassive;
            }

            if (account.Protocol != FtpProtocol.FTPS)
            {
                return;
            }

            switch (account.FTPSEncryption)
            {
                case FtpsEncryption.Implicit:
                    client.EncryptionMode = FtpEncryptionMode.Implicit;
                    break;
                case FtpsEncryption.Explicit:
                    client.EncryptionMode = FtpEncryptionMode.Explicit;
                    break;
            }

            client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
            client.DataConnectionEncryption = true;

            if (!string.IsNullOrEmpty(account.FTPSCertificateLocation) && System.IO.File.Exists(account.FTPSCertificateLocation))
            {
                var cert = X509Certificate.CreateFromSignedFile(account.FTPSCertificateLocation);
                client.ClientCertificates.Add(cert);
            }
            else
            {
                client.ValidateCertificate += (control, e) =>
                {
                    if (e.PolicyErrors != SslPolicyErrors.None)
                    {
                        e.Accept = true;
                    }
                };
            }
        }

        /// <inheritdoc />
        public override async Task<IUploadingResponse> UploadAsync(IFile file, CancellationToken cancellationToken)
        {
            var subFolderPath = account.Directory;
            var fullFileName = FtpHelper.GetFullFileName(file);
            var path = UrlHelper.CombineUrl(subFolderPath, fullFileName);

            IsUploading = true;
            Stream fileStream = null;

            try
            {
                fileStream = file.GetStream();
                if (await UploadDataAsync(fileStream, path, cancellationToken))
                {
                    return await base.UploadAsync(file, cancellationToken);
                }
            }
            catch (FtpCommandException e) when (isNeedToHandle)
            {
                // Probably directory not exist, try to create it.
                if (e.CompletionCode == "550" || e.CompletionCode == "553")
                {
                    isNeedToHandle = false;
                    await CreateMultiDirectoryAsync(UrlHelper.GetDirectoryPath(path), cancellationToken);
                    return await UploadAsync(file, cancellationToken);
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                }
            }

            IsUploading = false;
            return new BaseUploadingResponse(false);
        }

        /// <summary>
        /// Connects to destination FTP server.
        /// </summary>
        protected override async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            if (!client.IsConnected)
            {
                await client.ConnectAsync(cancellationToken);
            }

            return client.IsConnected;
        }

        /// <summary>
        /// Disconnects from destination FTP server.
        /// </summary>
        private async Task DisconnectAsync()
        {
            if (client != null && client.IsConnected)
            {
                await client.DisconnectAsync();
            }
        }

        private async Task<bool> UploadDataAsync(Stream stream, string remotePath, CancellationToken cancellationToken)
        {
            var result = await client.UploadAsync(stream, remotePath, FtpRemoteExists.Overwrite, true, null, cancellationToken);
            return result.IsSuccess();
        }

        /// <summary>
        /// Creates directory on remote FTP server.
        /// </summary>
        private async Task<bool> CreateDirectoryAsync(string remotePath, CancellationToken cancellationToken)
        {
            if (await ConnectAsync(cancellationToken))
            {
                try
                {
                    await client.CreateDirectoryAsync(remotePath, cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Error in creating directory on FTP server");
                }
            }

            return false;
        }

        /// <summary>
        /// Creates directories if need.
        /// </summary>
        private async Task CreateMultiDirectoryAsync(string remotePath, CancellationToken cancellationToken)
        {
            var paths = UrlHelper.GetPaths(remotePath);

            foreach (string directory in paths)
            {
                await CreateDirectoryAsync(directory, cancellationToken);
                logger.Info("Directory {directory} was created on server {server}", directory, account.Host);
            }
        }

        /// <summary>
        /// Disposes FTP client.
        /// </summary>
        public void Dispose()
        {
            if (disposed == false && client != null)
            {
                try
                {
                    client.Dispose();
                    client = null;
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error in disposing FTP client");
                }

                disposed = true;
            }
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(FtpUploader));
            }

            await DisconnectAsync();
            client.Dispose();
            client = null;

            disposed = true;
        }
    }
}
