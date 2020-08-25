using System;
using System.Collections.Generic;
using System.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.DataTransfer.Ftp;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    public class FtpConfiguration : IConfigurationOption, IEncryptable
    {
        public string UniqueName => "FTP accounts configuration";

        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();

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

        public IEncryptable Decrypt(IEncryptionService encryptionService)
        {
            FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Decrypt(a.Passphrase);
                a.Password = encryptionService.Decrypt(a.Password);
            });

            return this;
        }

        public FtpConfiguration Clone()
        {
            var clone = (FtpConfiguration)MemberwiseClone();
            clone.FtpAccounts = new List<FtpAccount>(FtpAccounts.Select(a => a.Clone()));

            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
