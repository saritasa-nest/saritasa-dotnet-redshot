using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Configuration.Models.Recording;

namespace RedShot.Infrastructure.Configuration.Providers
{
    /// <summary>
    /// Recording configuration provider.
    /// </summary>
    public class RecordingConfigurationProvider : ISpecificConfigurationProvider<RecordingConfiguration>
    {
        const string ConfigurationSectionName = "Recording";

        private JsonSerializer jsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordingConfigurationProvider(JsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        /// <inheritdoc />
        public RecordingConfiguration Get(JObject config)
        {
            if (!config.TryGetValue(ConfigurationSectionName, out var rawRecordingConfig))
            {
                return new RecordingConfiguration();
            }

            return rawRecordingConfig.ToObject<RecordingConfiguration>(jsonSerializer);
        }

        /// <inheritdoc />
        public JObject Update(RecordingConfiguration newValue, JObject currentConfig)
        {
            currentConfig.Remove(ConfigurationSectionName);
            currentConfig.Add(ConfigurationSectionName, JObject.FromObject(newValue, jsonSerializer));

            return currentConfig;
        }
    }
}
