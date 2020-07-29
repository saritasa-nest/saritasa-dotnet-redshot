using RedShot.Helpers.FtpModels;
using System.Collections.Generic;

namespace RedShot.Configuration
{
    public sealed class YamlConfig
    {
        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();
        public Dictionary<string, object> Extensions { get; internal set; } = new Dictionary<string, object>();
    }
}
