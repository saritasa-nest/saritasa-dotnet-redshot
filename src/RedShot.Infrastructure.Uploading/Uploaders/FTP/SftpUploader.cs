using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Basics;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// SFTP uploader.
    /// </summary>
    internal sealed class SftpUploader : BaseUploader, IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string fileName;
        private bool disposed;
        private SftpClient client;

        /// <summary>
        /// Initializes SFTP uploader.
        /// </summary>
        public SftpUploader(FtpAccount account, string fileName)
        {
            this.account = account;
            this.fileName = fileName;
        }

        private readonly FtpAccount account;

        /// <summary>
        /// Connection flag.
        /// </summary>
        private bool IsConnected => client != null && client.IsConnected;

        /// <inheritdoc cref="BaseUploader"/>.
        public override IUploadingResponse Upload(IFile file)
        {
            string subFolderPath = account.SubFolderPath;

            string path;
            if (string.IsNullOrEmpty(fileName))
            {
                path = UrlHelper.CombineUrl(subFolderPath, Path.GetFileName(file.FilePath));
            }
            else
            {
                path = UrlHelper.CombineUrl(subFolderPath, $"{fileName}{Path.GetExtension(file.FilePath)}");
            }

            IsUploading = true;

            try
            {
                if (UploadStream(file.GetStream(), path, true))
                {
                    return new BaseUploadingResponse(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while SFTP client was uploading data");
            }
            finally
            {
                Dispose();
                IsUploading = false;
            }

            return new BaseUploadingResponse(false);
        }

        /// <inheritdoc cref="BaseUploader"/>.
        public override void StopUpload()
        {
            if (IsUploading && !StopUploadRequested)
            {
                StopUploadRequested = true;

                try
                {
                    Disconnect();
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error while uploading was stopping");
                }
            }
        }

        /// <summary>
        /// Connects to destination FTP server.
        /// </summary>
        private bool Connect()
        {
            if (client == null)
            {
                if (!string.IsNullOrEmpty(account.Keypath))
                {
                    if (!System.IO.File.Exists(account.Keypath))
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
            }

            if (client != null && !client.IsConnected)
            {
                client.Connect();
            }

            return IsConnected;
        }

        /// <summary>
        /// Disconnects from destination FTP server.
        /// </summary>
        private void Disconnect()
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Checks if directory exists.
        /// </summary>
        private bool DirectoryExists(string path)
        {
            if (Connect())
            {
                return client.Exists(path);
            }

            return false;
        }

        /// <summary>
        /// Creates directory on remote SFTP server.
        /// </summary>
        private void CreateDirectory(string path, bool createMultiDirectory = false)
        {
            if (Connect())
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

        /// <summary>
        /// Creates directories if need.
        /// </summary>
        private List<string> CreateMultiDirectory(string path)
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

        private bool UploadStream(Stream stream, string remotePath, bool autoCreateDirectory = false)
        {
            if (Connect())
            {
                try
                {
                    client.UploadFile(stream, remotePath);
                    return true;
                }
                catch (SftpPathNotFoundException) when (autoCreateDirectory)
                {
                    Logger.Info($"Directory not exist, path:{remotePath}");
                    CreateDirectory(UrlHelper.GetDirectoryPath(remotePath), true);
                    return UploadStream(stream, remotePath);
                }
                catch (NullReferenceException)
                {
                    Logger.Warn("Error occurred by disconnect while uploading");
                }
            }

            return false;
        }

        /// <summary>
        /// Disposes SFTP client.
        /// </summary>
        public void Dispose()
        {
            if (disposed == false && client != null)
            {
                try
                {
                    client.Dispose();
                }
                catch (Exception e)
                {
                    client = null;
                    Logger.Error(e, "Error in disposing FTP client");
                }

                disposed = true;
            }
        }
    }
}
