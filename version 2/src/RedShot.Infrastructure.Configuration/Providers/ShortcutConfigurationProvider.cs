using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Configuration.Models.Shortcut;

namespace RedShot.Infrastructure.Configuration.Providers
{
    /// <summary>
    /// Shortcut configuration provider.
    /// </summary>
    public class ShortcutConfigurationProvider : ISpecificConfigurationProvider<ShortcutConfiguration>
    {
        const string ConfigurationSectionName = "Shortcut";

        private JsonSerializer jsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShortcutConfigurationProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        /// <inheritdoc />
        public ShortcutConfiguration Get(JObject config)
        {
            if (!config.TryGetValue(ConfigurationSectionName, out var rawShortcutConfig))
            {
                return new ShortcutConfiguration();
            }

            return rawShortcutConfig.ToObject<ShortcutConfiguration>(jsonSerializer);
        }

        /// <inheritdoc />
        public JObject Update(ShortcutConfiguration newValue, JObject currentConfig)
        {
            currentConfig.Remove(ConfigurationSectionName);
            currentConfig.Add(ConfigurationSectionName, JObject.FromObject(newValue, jsonSerializer));

            return currentConfig;
        }
    }
}
