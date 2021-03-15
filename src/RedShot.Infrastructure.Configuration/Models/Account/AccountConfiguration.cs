using System;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Configuration.Models.Account
{
    /// <summary>
    /// Contains a configuration for user accounts.
    /// </summary>
    public class AccountConfiguration
    {
        /// <summary>
        /// Id of the primary account.
        /// </summary>
        public Guid PrimaryAccountGuid { get; set; }

        /// <summary>
        /// List of available accounts.
        /// </summary>
        public IList<AccountData> Accounts { get; set; }
    }
}
