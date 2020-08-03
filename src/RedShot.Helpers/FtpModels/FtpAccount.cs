using System;

namespace RedShot.Helpers.FtpModels
{
    /// <summary>
    /// Ftp account model.
    /// </summary>
    public class FtpAccount
    {
        /// <summary>
        /// Inits base values.
        /// </summary>
        public FtpAccount()
        {
            Name = "FTP account";
            Protocol = FtpProtocol.FTP;
            Host = "";
            Port = 21;
            IsActive = false;
            SubFolderPath = "";
            FTPSEncryption = FtpsEncryption.Explicit;
            FTPSCertificateLocation = "";
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Unique Id of the account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ftp protocol of the account.
        /// </summary>
        public FtpProtocol Protocol { get; set; }

        /// <summary>
        /// Host of account's FTP server.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port of account's FTP server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Username of the account.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password of the account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Active flag.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Sub folder path on FTP server.
        /// </summary>
        public string SubFolderPath { get; set; }

        /// <summary>
        /// FTP adress property.
        /// </summary>
        public string FTPAddress
        {
            get
            {
                if (string.IsNullOrEmpty(Host))
                {
                    return "";
                }

                string serverProtocol;

                switch (Protocol)
                {
                    default:
                    case FtpProtocol.FTP:
                        serverProtocol = "ftp://";
                        break;
                    case FtpProtocol.FTPS:
                        serverProtocol = "ftps://";
                        break;
                    case FtpProtocol.SFTP:
                        serverProtocol = "sftp://";
                        break;
                }

                return string.Format("{0}{1}:{2}", serverProtocol, Host, Port);
            }
        }

        /// <summary>
        /// FTPS encryption method.
        /// </summary>
        public FtpsEncryption FTPSEncryption { get; set; }

        /// <summary>
        /// FTPS Certificate location.
        /// </summary>
        public string FTPSCertificateLocation { get; set; }

        /// <summary>
        /// Keypath.
        /// </summary>
        public string Keypath { get; set; }

        /// <summary>
        /// Passphrase.
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// Return understandable full name of the account.
        /// </summary>
        public override string ToString()
        {
            return $"{Name}||({Host}:{Port})";
        }
    }
}
