using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using FluentFTP;
using RedShot.Abstractions.Uploading;
using RedShot.Helpers;
using RedShot.Helpers.FtpModels;
using RedShot.Upload.Basics;

namespace RedShot.Upload.Uploaders.FTP
{
    /// <summary>
    /// FTP/FTPS uploader.
    /// </summary>
    public sealed class Ftp : BaseUploader, IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private FtpClient client;
        private bool disposed;

        public override event EventHandler Uploaded;

        public override event EventHandler UploadStoped;

        public override event EventHandler UploadStarted;

        public FtpAccount Account { get; private set; }

        public bool IsConnected
        {
            get
            {
                return client != null && client.IsConnected;
            }
        }

        public Ftp(FtpAccount account)
        {
            Account = account;

            client = new FtpClient()
            {
                Host = Account.Host,
                Port = Account.Port,
                Credentials = new NetworkCredential(Account.Username, Account.Password)
            };

            if (account.IsActive)
            {
                client.DataConnectionType = FtpDataConnectionType.AutoActive;
            }
            else
            {
                client.DataConnectionType = FtpDataConnectionType.AutoPassive;
            }

            if (account.Protocol == FtpProtocol.FTPS)
            {
                switch (Account.FTPSEncryption)
                {
                    default:
                    case FtpsEncryption.Explicit:
                        client.EncryptionMode = FtpEncryptionMode.Explicit;
                        break;
                    case FtpsEncryption.Implicit:
                        client.EncryptionMode = FtpEncryptionMode.Implicit;
                        break;
                }

                client.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                client.DataConnectionEncryption = true;

                if (!string.IsNullOrEmpty(account.FTPSCertificateLocation) && File.Exists(account.FTPSCertificateLocation))
                {
                    var cert = X509Certificate.CreateFromSignedFile(Account.FTPSCertificateLocation);
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
        }

        public override IUploaderResponse Upload(Stream stream, string fileName)
        {
            string subFolderPath = Account.SubFolderPath;
            string path = UrlHelper.CombineURL(subFolderPath, fileName);

            IsUploading = true;

            UploadStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                if (UploadData(stream, path))
                {
                    Uploaded?.Invoke(this, EventArgs.Empty);
                    return new BaseUploaderResponse(true);
                }
                else
                {
                    return new BaseUploaderResponse(false);
                }
            }
            catch (FtpCommandException e)
            {
                Logger.Warn(e, "Ftp command error");

                // Probably directory not exist, try creating it
                if (e.CompletionCode == "550" || e.CompletionCode == "553")
                {
                    CreateMultiDirectory(UrlHelper.GetDirectoryPath(path));

                    Upload(stream, fileName);
                }

                return new BaseUploaderResponse(false, path, errors: new List<string>(1) { e.Message });
            }
            finally
            {
                Dispose();
                IsUploading = false;
            }
        }

        public override void StopUpload()
        {
            if (IsUploading && !StopUploadRequested)
            {
                StopUploadRequested = true;
                Disconnect();
            }
        }

        public bool Connect()
        {
            if (!client.IsConnected)
            {
                client.Connect();
            }

            return client.IsConnected;
        }

        public void Disconnect()
        {
            if (client != null)
            {
                client.Disconnect();
            }
        }

        private bool UploadData(Stream stream, string remotePath)
        {
            var result = client.Upload(stream, remotePath, FtpRemoteExists.Overwrite, true);

            return result.IsSuccess();
        }

        public void UploadFiles(string[] localPaths, string remotePath)
        {
            foreach (string file in localPaths)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    string filename = Path.GetFileName(file);

                    if (File.Exists(file))
                    {
                        UploadFile(file, UrlHelper.CombineURL(remotePath, filename));
                    }
                    else if (Directory.Exists(file))
                    {
                        List<string> filesList = new List<string>();
                        filesList.AddRange(Directory.GetFiles(file));
                        filesList.AddRange(Directory.GetDirectories(file));
                        string path = UrlHelper.CombineURL(remotePath, filename);
                        CreateDirectory(path);
                        UploadFiles(filesList.ToArray(), path);
                    }
                }
            }
        }

        public IEnumerable<FtpListItem> GetListing(string remotePath)
        {
            return client.GetListing(remotePath);
        }

        public bool DirectoryExists(string remotePath)
        {
            if (Connect())
            {
                return client.DirectoryExists(remotePath);
            }

            return false;
        }

        public bool CreateDirectory(string remotePath)
        {
            if (Connect())
            {
                try
                {
                    client.CreateDirectory(remotePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Error in creating directory on FTP server");
                }
            }

            return false;
        }

        public IEnumerable<string> CreateMultiDirectory(string remotePath)
        {
            var paths = UrlHelper.GetPaths(remotePath);

            foreach (string path in paths)
            {
                CreateDirectory(path);
            }

            return paths;
        }

        public void Rename(string fromRemotePath, string toRemotePath)
        {
            if (Connect())
            {
                client.Rename(fromRemotePath, toRemotePath);
            }
        }

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
                    Logger.Error(e, "Error in disposing FTP client");
                }

                disposed = true;
            }
        }
    }
}
