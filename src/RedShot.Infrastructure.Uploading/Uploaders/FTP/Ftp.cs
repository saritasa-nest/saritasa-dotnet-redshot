using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using FluentFTP;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Upload.Basics;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// FTP/FTPS uploader.
    /// </summary>
    public sealed class Ftp : BaseUploader, IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly FtpAccount account;
        private FtpClient client;
        private bool disposed;

        /// <inheritdoc cref="BaseUploader"/>
        public override event EventHandler Uploaded;

        /// <inheritdoc cref="BaseUploader"/>
        public override event EventHandler UploadStoped;

        /// <inheritdoc cref="BaseUploader"/>.
        public override event EventHandler UploadStarted;

        /// <summary>
        /// Connection flag.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return client != null && client.IsConnected;
            }
        }

        /// <summary>
        /// Initializes FTP/FTPS uploader.
        /// </summary>
        public Ftp(FtpAccount account)
        {
            this.account = account;

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

            if (account.Protocol == FtpProtocol.FTPS)
            {
                switch (account.FTPSEncryption)
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
        }

        /// <inheritdoc cref="BaseUploader"/>.
        public override IUploadingResponse Upload(IFile file)
        {
            string subFolderPath = account.SubFolderPath;
            string path = UrlHelper.CombineURL(subFolderPath, file.FileName);

            IsUploading = true;

            UploadStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                if (UploadData(file.GetStream(), path))
                {
                    Uploaded?.Invoke(this, EventArgs.Empty);
                    return new BaseUploadingResponse(true);
                }
                else
                {
                    return new BaseUploadingResponse(false);
                }
            }
            catch (FtpCommandException e)
            {
                Logger.Warn(e, "FTP command error");

                // Probably directory not exist, try creating it
                if (e.CompletionCode == "550" || e.CompletionCode == "553")
                {
                    CreateMultiDirectory(UrlHelper.GetDirectoryPath(path));

                    Upload(file);
                }

                return new BaseUploadingResponse(false, path, errors: new List<string>(1) { e.Message });
            }
            finally
            {
                Dispose();
                IsUploading = false;
            }
        }

        /// <inheritdoc cref="BaseUploader"/>.
        public override void StopUpload()
        {
            if (IsUploading && !StopUploadRequested)
            {
                StopUploadRequested = true;
                Disconnect();
            }
        }

        /// <summary>
        /// Connects to destination FTP server.
        /// </summary>
        public bool Connect()
        {
            if (!client.IsConnected)
            {
                client.Connect();
            }

            return client.IsConnected;
        }

        /// <summary>
        /// Disconnects from destination FTP server.
        /// </summary>
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

        /// <summary>
        /// Creates directory on remote FTP server.
        /// </summary>
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

        /// <summary>
        /// Creates directories if need.
        /// </summary>
        public IEnumerable<string> CreateMultiDirectory(string remotePath)
        {
            var paths = UrlHelper.GetPaths(remotePath);

            foreach (string path in paths)
            {
                CreateDirectory(path);
            }

            return paths;
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
                    Logger.Error(e, "Error in disposing FTP client");
                }

                disposed = true;
            }
        }
    }
}
