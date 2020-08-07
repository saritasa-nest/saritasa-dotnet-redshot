using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Helpers.FtpModels
{
    /// <summary>
    /// Ftp account model.
    /// </summary>
    public class FtpAccount : INotifyPropertyChanged
    {
        private string name;
        private FtpProtocol protocol;
        private string host;
        private int port;
        private string username;
        private string password;
        private bool isActive;
        private string subFolderPath;
        private FtpsEncryption ftpsEncryption;
        private string ftpsCertificateLocation;
        private string keypath;
        private string passphrase;

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
        public string Name
        {
            get { return name; }

            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Ftp protocol of the account.
        /// </summary>
        public FtpProtocol Protocol
        {
            get { return protocol; }

            set
            {
                if (protocol != value)
                {
                    protocol = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Host of account's FTP server.
        /// </summary>
        public string Host
        {
            get { return host; }

            set
            {
                if (host != value)
                {
                    host = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Port of account's FTP server.
        /// </summary>
        public int Port
        {
            get { return port; }

            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Username of the account.
        /// </summary>
        public string Username
        {
            get { return username; }

            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Password of the account.
        /// </summary>
        public string Password
        {
            get { return password; }

            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Active flag.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }

            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sub folder path on FTP server.
        /// </summary>
        public string SubFolderPath
        {
            get { return subFolderPath; }

            set
            {
                if (subFolderPath != value)
                {
                    subFolderPath = value;
                    OnPropertyChanged();
                }
            }
        }

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
        public FtpsEncryption FTPSEncryption
        {
            get { return ftpsEncryption; }

            set
            {
                if (ftpsEncryption != value)
                {
                    ftpsEncryption = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// FTPS Certificate location.
        /// </summary>
        public string FTPSCertificateLocation
        {
            get { return ftpsCertificateLocation; }

            set
            {
                if (ftpsCertificateLocation != value)
                {
                    ftpsCertificateLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Keypath.
        /// </summary>
        public string Keypath
        {
            get { return keypath; }

            set
            {
                if (keypath != value)
                {
                    keypath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Passphrase.
        /// </summary>
        public string Passphrase
        {
            get { return passphrase; }

            set
            {
                if (passphrase != value)
                {
                    passphrase = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Return understandable full name of the account.
        /// </summary>
        public override string ToString()
        {
            return $"{Name}||({Host}:{Port})";
        }

        void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
