using RedShot.Infrastructure.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Infrastructure.DataTransfer.Ftp
{
    public class FtpConfiguration : IConfigurationSection
    {
        public static string ConfigName => "FTP accounts config";

        public string UniqueName => ConfigName;

        public List<FtpAccount> FtpAccounts { get; }

        public IConfigurationSection DecodeSection(IEncryptionService encryptionService)
        {
            FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Decrypt(a.Passphrase);
                a.Password = encryptionService.Decrypt(a.Password);
            });

            return this;
        }

        public IConfigurationSection EncodeSection(IEncryptionService encryptionService)
        {
            var encoded = (FtpConfiguration)this.MemberwiseClone();

            encoded.FtpAccounts.Clear();
            encoded.FtpAccounts.AddRange(FtpAccounts.Select(a => a.Clone()));
            encoded.FtpAccounts.ForEach(a =>
            {
                a.Passphrase = encryptionService.Encrypt(a.Passphrase);
                a.Password = encryptionService.Encrypt(a.Password);
            });

            return encoded;
        }
    }
}
