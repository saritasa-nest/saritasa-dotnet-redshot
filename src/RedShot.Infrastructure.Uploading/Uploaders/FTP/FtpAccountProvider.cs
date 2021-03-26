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
    public class FtpAccountProvider
    {
        /// <summary>
        /// Instance of FTP account provider.
        /// </summary>
        public static FtpAccountProvider Instance { get; } = new FtpAccountProvider();

        /// <summary>
        /// Get primary account.
        /// </summary>
        public FtpAccount GetPrimaryFtpAccount()
        {
            var config = GetConfiguration();
            var account = GetFtpAccounts().FirstOrDefault(a => a.Id == config.PrimaryAccountGuid);

            return account;
        }

        /// <summary>
        /// Get FTP accounts.
        /// </summary>
        public IEnumerable<FtpAccount> GetFtpAccounts()
        {
            var config = GetConfiguration();
            return config.FtpAccounts;
        }

        /// <summary>
        /// Get FTP account manually.
        /// </summary>
        public FtpAccount GetFtpAccountManually()
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

        private FtpConfiguration GetConfiguration()
        {
            return UserConfiguration.Instance.GetOptionOrDefault<FtpConfiguration>();
        }
    }
}
