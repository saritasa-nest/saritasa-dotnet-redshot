using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using RedShot.Helpers;
using RedShot.Helpers.FtpModels;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Basics;

namespace RedShot.Upload.Uploaders.FTP
{
    /// <summary>
    /// SFTP uploader.
    /// </summary>
    public sealed class Sftp : BaseUploader, IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private bool disposed;
        private SftpClient client;

        /// <summary>
        /// Initializes SFTP uploader.
        /// </summary>
        public Sftp(FtpAccount account)
        {
            this.account = account;
        }

        /// <inheritdoc cref="BaseUploader"/>.
        public override event EventHandler Uploaded;

        /// <inheritdoc cref="BaseUploader"/>.
        public override event EventHandler UploadStoped;

        /// <inheritdoc cref="BaseUploader"/>.
        public override event EventHandler UploadStarted;

        private FtpAccount account;

        /// <summary>
        /// Valid account flag.
        /// </summary>
        public bool IsValidAccount => (!string.IsNullOrEmpty(account.Keypath) && File.Exists(account.Keypath)) || !string.IsNullOrEmpty(account.Password);

        /// <summary>
        /// Connection flag.
        /// </summary>
        public bool IsConnected => client != null && client.IsConnected;

        /// <inheritdoc cref="BaseUploader"/>.
        public override IUploaderResponse Upload(Stream stream, string fileName)
        {
            string subFolderPath = account.SubFolderPath;
            string path = UrlHelper.CombineURL(subFolderPath, fileName);

            IsUploading = true;

            UploadStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                if (UploadStream(stream, path, true))
                {
                    Uploaded?.Invoke(this, EventArgs.Empty);
                    return new BaseUploaderResponse(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occured while SFTP client was uploading data");
            }
            finally
            {
                Dispose();
                IsUploading = false;
            }

            return new BaseUploaderResponse(false);
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
        public bool Connect()
        {
            if (client == null)
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
        public void Disconnect()
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Checks if directory exists.
        /// </summary>
        public bool DirectoryExists(string path)
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
        public void CreateDirectory(string path, bool createMultiDirectory = false)
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
        public List<string> CreateMultiDirectory(string path)
        {
            List<string> directoryList = new List<string>();

            List<string> paths = UrlHelper.GetPaths(path);

            foreach (string directory in paths)
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
                    Logger.Warn("Error occured by disconnect while uploading");
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
