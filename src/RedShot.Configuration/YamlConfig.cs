using RedShot.Helpers.FtpModels;
using System.Collections.Generic;

namespace RedShot.Configuration
{
    /// <summary>
    /// Configuration object.
    /// </summary>
    public sealed class YamlConfig
    {
        /// <summary>
        /// Ftp accounts.
        /// </summary>
        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();

        /// <summary>
        /// Some extensions if need.
        /// </summary>
        public Dictionary<string, object> Extensions { get; internal set; } = new Dictionary<string, object>();
    }
}
