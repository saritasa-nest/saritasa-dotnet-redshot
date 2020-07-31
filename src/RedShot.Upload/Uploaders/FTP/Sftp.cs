using RedShot.Helpers;
using RedShot.Helpers.FtpModels;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Basics;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace RedShot.Upload.Uploaders.FTP
{
    public sealed class Sftp : BaseUploader, IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private bool disposed;
        private SftpClient client;

        public Sftp(FtpAccount account)
        {
            Account = account;
        }

        public override event EventHandler Uploaded;

        public override event EventHandler UploadStoped;

        public override event EventHandler UploadStarted;

        public FtpAccount Account { get; private set; }

        public bool IsValidAccount => (!string.IsNullOrEmpty(Account.Keypath) && File.Exists(Account.Keypath)) || !string.IsNullOrEmpty(Account.Password);

        public bool IsConnected => client != null && client.IsConnected;

        public override IUploaderResponse Upload(Stream stream, string fileName)
        {
            string subFolderPath = Account.SubFolderPath;
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

        public bool Connect()
        {
            if (client == null)
            {
                if (!string.IsNullOrEmpty(Account.Keypath))
                {
                    if (!File.Exists(Account.Keypath))
                    {
                        throw new Exception("Key not found");
                    }

                    PrivateKeyFile keyFile;

                    if (string.IsNullOrEmpty(Account.Passphrase))
                    {
                        keyFile = new PrivateKeyFile(Account.Keypath);
                    }
                    else
                    {
                        keyFile = new PrivateKeyFile(Account.Keypath, Account.Passphrase);
                    }

                    client = new SftpClient(Account.Host, Account.Port, Account.Username, keyFile);
                }
                else if (!string.IsNullOrEmpty(Account.Password))
                {
                    client = new SftpClient(Account.Host, Account.Port, Account.Username, Account.Password);
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

        public void Disconnect()
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
        }

        public void ChangeDirectory(string path, bool autoCreateDirectory = false)
        {
            if (Connect())
            {
                try
                {
                    client.ChangeDirectory(path);
                }
                catch (SftpPathNotFoundException ex) when (autoCreateDirectory)
                {
                    Logger.Warn(ex);
                    CreateDirectory(path, true);
                    ChangeDirectory(path);
                }
            }
        }

        public bool DirectoryExists(string path)
        {
            if (Connect())
            {
                return client.Exists(path);
            }

            return false;
        }

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
                    // Happens when directory not exist, create directory and retry uploading

                    CreateDirectory(UrlHelper.GetDirectoryPath(remotePath), true);
                    return UploadStream(stream, remotePath);
                }
                catch (NullReferenceException)
                {
                    // Happens when disconnect while uploading
                }
            }

            return false;
        }

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
