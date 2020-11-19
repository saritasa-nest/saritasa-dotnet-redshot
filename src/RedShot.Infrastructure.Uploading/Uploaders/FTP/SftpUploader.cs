using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Basics;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using System.Threading;
using System.Threading.Tasks;
using Saritasa.Tools.Domain.Exceptions;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// SFTP uploader.
    /// </summary>
    internal sealed class SftpUploader : BaseFtpUploader, IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly FtpAccount account;
        private bool disposed;
        private SftpClient client;
        private object lockObject = new object();

        /// <summary>
        /// Initializes SFTP uploader.
        /// </summary>
        public SftpUploader(FtpAccount account)
        {
            this.account = account;
            InitializeClient();
        }

        /// <inheritdoc/>
        public override async Task<IUploadingResponse> UploadAsync(IFile file, CancellationToken cancellationToken)
        {
            var subFolderPath = account.SubFolderPath;
            var fullFileName = FtpHelper.GetFullFileName(file);
            var path = UrlHelper.CombineUrl(subFolderPath, fullFileName);

            IsUploading = true;

            try
            {
                if (await UploadStreamAsync(file.GetStream(), path, true, cancellationToken))
                {
                    return await base.UploadAsync(file, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while SFTP client was uploading data");
            }
            finally
            {
                await DisconnectAsync();
                IsUploading = false;
            }

            return new BaseUploadingResponse(false);
        }

        /// <summary>
        /// Connects to destination FTP server.
        /// </summary>
        protected override async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            if (!client.IsConnected)
            {
                await Task.Factory.StartNew(() =>
                {
                    lock (lockObject)
                    {
                        if (client != null && !client.IsConnected)
                        {
                            client.Connect();
                        }
                    }
                });
            }

            return client.IsConnected;
        }

        private async Task<bool> UploadStreamAsync(Stream stream, string remotePath, bool autoCreateDirectory = false, CancellationToken cancellationToken = default)
        {
            if (await ConnectAsync(cancellationToken))
            {
                try
                {
                    var asyncResult = client.BeginUploadFile(stream, remotePath);
                    await Task.Factory.FromAsync(asyncResult, (ar) =>
                    {
                        client.EndUploadFile(ar);
                    });

                    return true;
                }
                catch (SftpPathNotFoundException) when (autoCreateDirectory)
                {
                    logger.Info($"Directory not exist, path:{remotePath}");
                    CreateDirectory(UrlHelper.GetDirectoryPath(remotePath), autoCreateDirectory);
                    return await UploadStreamAsync(stream, remotePath, autoCreateDirectory, cancellationToken);
                }
                catch (NullReferenceException)
                {
                    logger.Warn("Error occurred by disconnect while uploading");
                }
            }

            return false;
        }

        private async Task DisconnectAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                lock (lockObject)
                {
                    if (client != null && client.IsConnected)
                    {
                        client.Disconnect();
                    }
                }
            });
        }

        private bool DirectoryExists(string path)
        {
            if (ConnectAsync(default).GetAwaiter().GetResult())
            {
                return client.Exists(path);
            }

            return false;
        }

        private void CreateDirectory(string path, bool createMultiDirectory = false)
        {
            if (ConnectAsync(default).GetAwaiter().GetResult())
            {
                try
                {
                    client.CreateDirectory(path);
                }
                catch (SftpPathNotFoundException) when (createMultiDirectory)
                {
                    CreateMultiDirectory(path);
                }
                catch (SftpPermissionDeniedException)
                {
                }
            }
        }

        private IEnumerable<string> CreateMultiDirectory(string path)
        {
            var directoryList = new List<string>();

            var paths = UrlHelper.GetPaths(path);

            foreach (var directory in paths)
            {
                if (!DirectoryExists(directory))
                {
                    CreateDirectory(directory);
                    directoryList.Add(directory);
                }
            }

            return directoryList;
        }

        private void InitializeClient()
        {
            if (!string.IsNullOrEmpty(account.Keypath))
            {
                if (!File.Exists(account.Keypath))
                {
                    throw new Exception("Key not found");
                }

                PrivateKeyFile keyFile;

                if (string.IsNullOrEmpty(account.Passphrase))
                {
                    keyFile = new PrivateKeyFile(account.Keypath);
                }
                else
                {
                    keyFile = new PrivateKeyFile(account.Keypath, account.Passphrase);
                }

                client = new SftpClient(account.Host, account.Port, account.Username, keyFile);
            }
            else if (!string.IsNullOrEmpty(account.Password))
            {
                client = new SftpClient(account.Host, account.Port, account.Username, account.Password);
            }

            if (client != null)
            {
                client.BufferSize = (uint)BufferSize;
            }
            else
            {
                throw new DomainException("The SFTP client was not initialized. You need ti input password or key for this FTP account!");
            }
        }

        /// <summary>
        /// Disposes SFTP client.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                client.Dispose();
                client = null;
            }
            disposed = true;
        }
    }
}
