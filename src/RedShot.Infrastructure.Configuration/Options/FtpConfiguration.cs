using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.DataTransfer.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Infrastructure.Configuration.Options
{
    public class FtpConfiguration : IConfigurationOption
    {
        public static string ConfigName => "FTP accounts configuration";

        public string UniqueName => ConfigName;

        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();

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

        public IConfigurationOption DecodeSection(IEncryptionService encryptionService)
        {
            FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Decrypt(a.Passphrase);
                a.Password = encryptionService.Decrypt(a.Password);
            });

            return this;
        }

        public IConfigurationOption EncodeSection(IEncryptionService encryptionService)
        {
            var encoded = Clone();

            encoded.FtpAccounts = new List<FtpAccount>(FtpAccounts.Select(a => a.Clone()));
            encoded.FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Encrypt(a.Passphrase);
                a.Password = encryptionService.Encrypt(a.Password);
            });

            return encoded;
        }
    }
}
