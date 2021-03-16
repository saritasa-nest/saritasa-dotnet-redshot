﻿using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP/SFTP options.
    /// </summary>
    public class FtpOptions : ICloneable
    {
        /// <summary>
        /// Primary account guid.
        /// </summary>
        public Guid PrimaryAccountGuid { get; set; }

        /// <summary>
        /// List of FTP accounts.
        /// </summary>
        public List<FtpAccount> FtpAccounts { get; private set; } = new List<FtpAccount>();

        /// <summary>
        /// Return clone of the configuration option.
        /// </summary>
        public FtpOptions Clone()
        {
            var clone = (FtpOptions)MemberwiseClone();
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
