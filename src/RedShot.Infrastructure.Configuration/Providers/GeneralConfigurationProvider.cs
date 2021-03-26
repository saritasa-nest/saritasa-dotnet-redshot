using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Configuration.Models.General;

namespace RedShot.Infrastructure.Configuration.Providers
{
    /// <summary>
    /// Manages the general configuration storage.
    /// </summary>
    public class GeneralConfigurationProvider : IConfigurationProvider<GeneralConfiguration>
    {
        const string ConfigurationSectionName = "General";

        private JsonSerializer jsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GeneralConfigurationProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        /// <inheritdoc />
        public GeneralConfiguration Get(JObject config)
        {
            if (!config.TryGetValue(ConfigurationSectionName, out var rawGeneralConfig))
            {
                return new GeneralConfiguration();
            }

            return rawGeneralConfig.ToObject<GeneralConfiguration>(jsonSerializer);
        }

        /// <inheritdoc />
        public JObject Update(GeneralConfiguration newValue, JObject currentConfig)
        {
            currentConfig.Remove(ConfigurationSectionName);
            currentConfig.Add(ConfigurationSectionName, JObject.FromObject(newValue, jsonSerializer));

            return currentConfig;
        }
    }
}
