using RedShot.Helpers.FtpModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Configuration
{
    public sealed class YamlConfig
    {
        public List<FtpAccount> FtpAccounts { get; internal set; }
        public Dictionary<string, object> Extensions { get; internal set; }
    }
}
