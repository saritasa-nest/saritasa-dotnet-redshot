using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Forms;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP accounts manager.
    /// </summary>
    public static class FtpAccountManager
    {
        /// <summary>
        /// Get primary account.
        /// </summary>
        public static FtpAccount GetPrimaryFtpAccount()
        {
            var config = GetConfiguration();

            if (config.PrimaryAccountGuid != default && TryGetAccountByGuid(config.PrimaryAccountGuid, config.FtpAccounts, out var ftpAccount))
            {
                return ftpAccount;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get FTP accounts.
        /// </summary>
        public static IEnumerable<FtpAccount> GetFtpAccounts()
        {
            var config = GetConfiguration();
            return config.FtpAccounts;
        }

        /// <summary>
        /// Get FTP account manually.
        /// </summary>
        public static FtpAccount GetFtpAccountManually()
        {
            using (var form = new FtpAccountSelectionForm())
            {
                if (form.ShowModal() == DialogResult.Ok)
                {
                    return form.SelectedAccount;
                }
            }

            return null;
        }

        internal static bool TryGetAccountByGuid(Guid guid, IEnumerable<FtpAccount> ftpAccounts, out FtpAccount ftpAccount)
        {
            if (ftpAccounts.Any(a => a.Id == guid))
            {
                ftpAccount = ftpAccounts.Single(a => a.Id == guid);
                return true;
            }
            else
            {
                ftpAccount = null;
                return false;
            }
        }

        private static FtpConfiguration GetConfiguration()
        {
            return ConfigurationManager.GetSection<FtpConfiguration>();
        }
    }
}
