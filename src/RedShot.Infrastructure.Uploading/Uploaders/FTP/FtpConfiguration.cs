﻿using System;
using System.Collections.Generic;
using System.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Configuration;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP configuration.
    /// </summary>
    public class FtpConfiguration : IEncryptable, ICloneable, IConfigurationOption
    {
        /// <summary>
        /// Primary account guid.
        /// </summary>
        public Guid PrimaryAccountGuid { get; set; }

        /// <summary>
        /// List of FTP accounts.
        /// </summary>
        public List<FtpAccount> FtpAccounts { get; private set; } = new List<FtpAccount>();

        /// <inheritdoc/>
        public string UniqueName => "FtpConfiguration";

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

        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
