using RedShot.Infrastructure.Configuration.Json;
using RedShot.Infrastructure.Domain.Ftp;
using System;

namespace RedShot.Infrastructure.Configuration.Models.Account
{
    /// <summary>
    /// Contains details about a specific ftp/sftp/ftps account.
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountData()
        {
            Protocol = FtpProtocol.FTP;
            Port = 21;
            FtpsEncryption = FtpsEncryption.Explicit;
            Id = Guid.NewGuid();
            HttpHomePathAddExtension = true;
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
        [PropertyEncrypt]
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
        [PropertyEncrypt]
        public string Passphrase { get; set; }
    }
}
