using FluentFTP;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Ftp;
using RedShot.Infrastructure.DomainServices.Common.Helpers;
using Saritasa.Tools.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FtpInternalClient = FluentFTP.FtpClient;

namespace RedShot.Infrastructure.DomainServices.Services.Ftp
{
    internal class FtpClient : BaseFtpClient, IDisposable
    {
        private readonly FtpAccount ftpAccount;

        private FtpInternalClient client;
        private bool isNeedToHandle;
        private bool disposedValue;

        public FtpClient(
            IFormatNameService formatService, 
            FtpAccount ftpAccount) : base(formatService, ftpAccount)
        {
            this.ftpAccount = ftpAccount;

            isNeedToHandle = true;
            InitializeFtpClient();
        }

        private void InitializeFtpClient()
        {
            client = new FtpInternalClient()
            {
                Host = ftpAccount.Host,
                Port = ftpAccount.Port,
                Credentials = new NetworkCredential(ftpAccount.Username, ftpAccount.Password)
            };

            if (ftpAccount.IsActive)
            {
                client.DataConnectionType = FtpDataConnectionType.AutoActive;
            }
            else
            {
                client.DataConnectionType = FtpDataConnectionType.AutoPassive;
            }

            if (ftpAccount.Protocol != FtpProtocol.FTPS)
            {
                return;
            }

            switch (ftpAccount.FtpsEncryption)
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

            if (!string.IsNullOrEmpty(ftpAccount.FtpsCertificateLocation) && File.Exists(ftpAccount.FtpsCertificateLocation))
            {
                var cert = X509Certificate.CreateFromSignedFile(ftpAccount.FtpsCertificateLocation);
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

        /// <inheritdoc/>
        protected override async Task UploadStreamAsync(Stream fileStream, string remotePath, CancellationToken cancellationToken = default)
        {
            try
            {
                await client.UploadAsync(fileStream, remotePath, FtpRemoteExists.Overwrite, true, null, cancellationToken);
            }
            catch (FtpCommandException e) when (isNeedToHandle)
            {
                // Probably directory not exist, try to create it.
                if (e.CompletionCode == "550" || e.CompletionCode == "553")
                {
                    isNeedToHandle = false;
                    await CreateMultiDirectoryAsync(UrlHelper.GetDirectoryPath(remotePath), cancellationToken);
                    await UploadStreamAsync(fileStream, remotePath);
                }
            }
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
                    //logger.Warn(ex, "Error in creating directory on FTP server");
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
                //logger.Info("Directory {directory} was created on server {server}", directory, ftpAccount.Host);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
    }
}
