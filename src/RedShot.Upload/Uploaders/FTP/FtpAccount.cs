namespace RedShot.Upload.Uploaders.FTP
{
    public class FtpAccount
    {
        public FtpAccount()
        {
            Name = "FTP account";
            Protocol = FtpProtocol.FTP;
            Host = "";
            Port = 21;
            IsActive = false;
            SubFolderPath = "";
            BrowserProtocol = BrowserProtocol.http;
            HttpHomePath = "";
            HttpHomePathAutoAddSubFolderPath = true;
            HttpHomePathNoExtension = false;
            FTPSEncryption = FtpsEncryption.Explicit;
            FTPSCertificateLocation = "";
        }

        public string Name { get; set; }

        public FtpProtocol Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string SubFolderPath { get; set; }

        public BrowserProtocol BrowserProtocol { get; set; }

        public string HttpHomePath { get; set; }

        public bool HttpHomePathAutoAddSubFolderPath { get; set; }

        public bool HttpHomePathNoExtension { get; set; }

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

        public FtpsEncryption FTPSEncryption { get; set; }

        public string FTPSCertificateLocation { get; set; }

        public string Keypath { get; set; }

        public string Passphrase { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Host}:{Port})";
        }

        public FtpAccount Clone()
        {
            return MemberwiseClone() as FtpAccount;
        }
    }
}
