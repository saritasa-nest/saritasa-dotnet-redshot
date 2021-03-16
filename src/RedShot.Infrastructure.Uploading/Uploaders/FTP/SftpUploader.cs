﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;
using Saritasa.Tools.Domain.Exceptions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// SFTP uploader.
    /// </summary>
    internal sealed class SftpUploader : FtpUploaderBase, IDisposable, IAsyncDisposable
    {
        private const int BufferSize = 8192;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool disposed;
        private bool isNeedToHandle;
        private volatile SftpClient client;
        private readonly object lockObject = new object();

        /// <summary>
        /// Initializes SFTP uploader.
        /// </summary>
        public SftpUploader(FtpAccount ftpAccount) : base(ftpAccount)
        {
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
                    logger.Info("Directory not exist, path: {remotePath}", remotePath);
                    await CreateDirectoryAsync(UrlHelper.GetDirectoryPath(remotePath), cancellationToken);
                    await UploadStreamAsync(stream, remotePath, cancellationToken);
                }
            }
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

        private async Task<bool> DirectoryExistsAsync(string path, CancellationToken cancellationToken)
        {
            if (await ConnectAsync(cancellationToken))
            {
                return await Task.Factory.StartNew(() =>
                {
                    lock (lockObject)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return client.Exists(path);
                    }
                });
            }

            return false;
        }

        private async Task CreateDirectoryAsync(string path, CancellationToken cancellationToken = default)
        {
            if (await ConnectAsync(cancellationToken))
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        lock (lockObject)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            client.CreateDirectory(path);
                        }
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
                if (! await DirectoryExistsAsync(directory, cancellationToken))
                {
                    await CreateDirectoryAsync(directory, cancellationToken);
                    logger.Info("Directory: {directory} was created on server: {server}", directory, ftpAccount.Host);
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

                client = new SftpClient(ftpAccount.Host, ftpAccount.Port, ftpAccount.Username, keyFile);
            }
            else if (!string.IsNullOrEmpty(ftpAccount.Password))
            {
                client = new SftpClient(ftpAccount.Host, ftpAccount.Port, ftpAccount.Username, ftpAccount.Password);
            }

            if (client != null)
            {
                client.BufferSize = (uint)BufferSize;
            }
            else
            {
                throw new DomainException("The SFTP client was not initialized. Password or RSA key is invalid.");
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

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(SftpUploader));
            }

            await Task.Factory.StartNew(() =>
            {
                lock (lockObject)
                {
                    client.Dispose();
                    client = null;
                }
            });

            disposed = true;
        }
    }
}
