using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using RedShot.Infrastructure.Configuration.Models.Account;

namespace RedShot.Infrastructure.Configuration.Providers
{
    /// <summary>
    /// Manages the account configuration storage.
    /// </summary>
    internal class AccountConfigurationProvider : ISpecificConfigurationProvider<AccountConfiguration>
    {
        const string ConfigurationSectionName = "Accounts";

        private JsonSerializer jsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountConfigurationProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        /// <inheritdoc />
        public AccountConfiguration Get(JObject config)
        {
            if (!config.TryGetValue(ConfigurationSectionName, out var rawAccountConfig))
            {
                return new AccountConfiguration()
                {
                    Accounts = new List<AccountData>()
                };
            }

            var accountConfig = rawAccountConfig.ToObject<AccountConfiguration>(jsonSerializer);
            if (accountConfig.Accounts == null)
            {
                accountConfig.Accounts = new List<AccountData>();
            }

            return accountConfig;
        }

        /// <inheritdoc />
        public JObject Update(AccountConfiguration newValue, JObject currentConfig)
        {
            currentConfig.Remove(ConfigurationSectionName);
            currentConfig.Add(ConfigurationSectionName, JObject.FromObject(newValue, jsonSerializer));

            return currentConfig;
        }
    }
}
