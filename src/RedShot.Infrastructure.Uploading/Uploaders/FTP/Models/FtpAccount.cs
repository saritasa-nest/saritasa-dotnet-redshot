using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models
{
    /// <summary>
    /// FTP account model.
    /// </summary>
    public class FtpAccount : INotifyPropertyChanged, ICloneable
    {
        private string name;
        private FtpProtocol protocol;
        private string host;
        private int port;
        private string username;
        private string password;
        private bool isActive;
        private string directory;
        private FtpsEncryption ftpsEncryption;
        private string ftpsCertificateLocation;
        private string keypath;
        private string passphrase;
        private string httpHomePath;
        private bool httpHomePathAddExtension;
        private BrowserProtocol browserProtocol;

        /// <summary>
        /// Inits base values.
        /// </summary>
        public FtpAccount()
        {
            Protocol = FtpProtocol.FTP;
            Host = "";
            Port = 21;
            IsActive = false;
            Directory = "";
            FTPSEncryption = FtpsEncryption.Explicit;
            FTPSCertificateLocation = "";
            Id = Guid.NewGuid();
            HttpHomePath = "";
            HttpHomePathAddExtension = false;
        }

        public string GetFormatLink(string fileName)
        {
            if (string.IsNullOrEmpty(HttpHomePath))
            {
                return "Empty URL";
            }

            var builder = new StringBuilder();
            builder.Append($"{HttpHomePath}/");

            if (HttpHomePathAddExtension)
            {
                builder.Append(fileName);
            }
            else
            {
                builder.Append(Path.GetFileNameWithoutExtension(fileName));
            }

            return builder.ToString();
        }

        /// <summary>
        /// HTTP home path no extension.
        /// </summary>
        public bool HttpHomePathAddExtension
        {
            get { return httpHomePathAddExtension; }

            set
            {
                if (httpHomePathAddExtension != value)
                {
                    httpHomePathAddExtension = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// HTTP home path.
        /// </summary>
        public string HttpHomePath
        {
            get { return httpHomePath; }

            set
            {
                if (httpHomePath != value)
                {
                    httpHomePath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Unique Id of the account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FTP protocol of the account.
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
        /// User name of the account.
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
        /// Directory path on FTP server.
        /// </summary>
        public string Directory
        {
            get { return directory; }

            set
            {
                if (directory != value)
                {
                    directory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// FTP address property.
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
        /// Key path.
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
        /// Pass phrase.
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
            return $"{Protocol} {Host}";
        }

        void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FtpAccount Clone()
        {
            return MemberwiseClone() as FtpAccount;
        }

        public override bool Equals(object obj)
        {
            if (obj is FtpAccount account)
            {
                return this.Id == account.Id;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
