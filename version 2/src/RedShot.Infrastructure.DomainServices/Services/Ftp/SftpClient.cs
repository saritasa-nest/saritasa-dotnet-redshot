using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Ftp;
using RedShot.Infrastructure.DomainServices.Common.Helpers;
using Renci.SshNet;
using Renci.SshNet.Common;
using Saritasa.Tools.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InternalSftpClient = Renci.SshNet.SftpClient;

namespace RedShot.Infrastructure.DomainServices.Services.Ftp
{
    internal class SftpClient : BaseFtpClient, IDisposable
    {
        private const uint BufferSize = 8192;

        private readonly FtpAccount ftpAccount;

        private bool isNeedToHandle;
        private volatile InternalSftpClient client;
        private bool disposedValue;

        /// <summary>
        /// Initializes SFTP uploader.
        /// </summary>
        public SftpClient(
            IFormatNameService formatService,
            FtpAccount ftpAccount) : base(formatService, ftpAccount)
        {
            this.ftpAccount = ftpAccount;

            isNeedToHandle = true;
            InitializeClient();
        }

        /// <inheritdoc/>
        protected override async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            if (!client.IsConnected)
            {
                await Task.Factory.StartNew(() =>
                {
                    if (client != null && !client.IsConnected)
                    {
                        client.Connect();
                    }
                });
            }

            return client.IsConnected;
        }

        /// <inheritdoc/>
        protected override async Task UploadStreamAsync(Stream stream, string remotePath, CancellationToken cancellationToken = default)
        {
            if (await ConnectAsync(cancellationToken))
            {
                try
                {
                    await Task.Factory.FromAsync(client.BeginUploadFile(stream, remotePath), client.EndUploadFile);
                }
                catch (SftpPathNotFoundException) when (isNeedToHandle)
                {
                    isNeedToHandle = false;
                    //logger.Info("Directory not exist, path: {remotePath}", remotePath);
                    await CreateDirectoryAsync(UrlHelper.GetDirectoryPath(remotePath), cancellationToken);
                    await UploadStreamAsync(stream, remotePath, cancellationToken);
                }
            }
        }

        private async Task<bool> DirectoryExistsAsync(string path, CancellationToken cancellationToken)
        {
            var result = false;

            if (await ConnectAsync(cancellationToken))
            {
                result = await Task.Factory.StartNew(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return client.Exists(path);
                });
            }

            return result;
        }

        private async Task CreateDirectoryAsync(string path, CancellationToken cancellationToken = default)
        {
            if (await ConnectAsync(cancellationToken))
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        client.CreateDirectory(path);
                    });
                }
                catch (SftpPathNotFoundException)
                {
                    await CreateMultiDirectoryAsync(path, cancellationToken);
                }
            }
        }

        private async Task CreateMultiDirectoryAsync(string path, CancellationToken cancellationToken)
        {
            var paths = UrlHelper.GetPaths(path);

            foreach (var directory in paths)
            {
                if (!await DirectoryExistsAsync(directory, cancellationToken))
                {
                    await CreateDirectoryAsync(directory, cancellationToken);
                    //logger.Info("Directory: {directory} was created on server: {server}", directory, ftpAccount.Host);
                }
            }
        }

        private void InitializeClient()
        {
            if (!string.IsNullOrEmpty(ftpAccount.Keypath))
            {
                if (!System.IO.File.Exists(ftpAccount.Keypath))
                {
                    throw new Exception("Key not found");
                }

                PrivateKeyFile keyFile;

                if (string.IsNullOrEmpty(ftpAccount.Passphrase))
                {
                    keyFile = new PrivateKeyFile(ftpAccount.Keypath);
                }
                else
                {
                    keyFile = new PrivateKeyFile(ftpAccount.Keypath, ftpAccount.Passphrase);
                }

                client = new InternalSftpClient(ftpAccount.Host, ftpAccount.Port, ftpAccount.Username, keyFile);
            }
            else if (!string.IsNullOrEmpty(ftpAccount.Password))
            {
                client = new InternalSftpClient(ftpAccount.Host, ftpAccount.Port, ftpAccount.Username, ftpAccount.Password);
            }

            if (client != null)
            {
                client.BufferSize = BufferSize;
            }
            else
            {
                throw new DomainException("The SFTP client was not initialized. Password or RSA key is invalid.");
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
