using System;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Domain.Ftp
{
    /// <summary>
    /// FTP account model.
    /// </summary>
    public class FtpAccount : ICloneable
    {
        /// <summary>
        /// Get format link.
        /// </summary>
        /// <param name="fileName">File name.</param>
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
        public bool HttpHomePathAddExtension { get; set; }

        /// <summary>
        /// HTTP home path.
        /// </summary>
        public string HttpHomePath { get; set; }

        /// <summary>
        /// Unique Id of the account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FTP protocol of the account.
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
        /// User name of the account.
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
        /// Directory path on FTP server.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// FTP address property.
        /// </summary>
        public string FTPAddress { get; set; }

        /// <summary>
        /// FTPS encryption method.
        /// </summary>
        public FtpsEncryption FtpsEncryption { get; set; }

        /// <summary>
        /// FTPS Certificate location.
        /// </summary>
        public string FtpsCertificateLocation { get; set; }

        /// <summary>
        /// Key path.
        /// </summary>
        public string Keypath { get; set; }

        /// <summary>
        /// Pass phrase.
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// Return understandable full name of the account.
        /// </summary>
        public override string ToString()
        {
            return $"{Protocol} {Host}";
        }

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
