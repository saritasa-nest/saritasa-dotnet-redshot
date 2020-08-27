using System;
using System.Collections.Generic;
using System.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.DataTransfer.Ftp;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// FTP configuration.
    /// </summary>
    public class FtpConfiguration : IConfigurationOption, IEncryptable
    {
        /// <inheritdoc cref="IConfigurationOption"/>
        public string UniqueName => "FTP accounts configuration";

        /// <summary>
        /// List of FTP accounts.
        /// </summary>
        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();

        /// <inheritdoc cref="IEncryptable"/>
        public IEncryptable Encrypt(IEncryptionService encryptionService)
        {
            var encrypted = Clone();

            encrypted.FtpAccounts = new List<FtpAccount>(FtpAccounts.Select(a => a.Clone()));
            encrypted.FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Encrypt(a.Passphrase);
                a.Password = encryptionService.Encrypt(a.Password);
            });

            return encrypted;
        }

        /// <inheritdoc cref="IEncryptable"/>
        public IEncryptable Decrypt(IEncryptionService encryptionService)
        {
            FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Decrypt(a.Passphrase);
                a.Password = encryptionService.Decrypt(a.Password);
            });

            return this;
        }

        /// <summary>
        /// Return clone of the configuration option.
        /// </summary>
        public FtpConfiguration Clone()
        {
            var clone = (FtpConfiguration)MemberwiseClone();
            clone.FtpAccounts = new List<FtpAccount>(FtpAccounts.Select(a => a.Clone()));

            return clone;
        }

        /// <inheritdoc cref="ICloneable"/>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
