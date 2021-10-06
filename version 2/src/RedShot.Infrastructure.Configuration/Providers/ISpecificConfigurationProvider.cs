using Newtonsoft.Json.Linq;

namespace RedShot.Infrastructure.Configuration.Providers
{
    /// <summary>
    /// A fictive base interface for all configuration providers.
    /// </summary>
    internal interface ISpecificConfigurationProvider
    {
    }

    /// <summary>
    /// Manages a specific type of configuration.
    /// </summary>
    /// <typeparam name="T">The configuration this object is parsing/saving.</typeparam>
    internal interface ISpecificConfigurationProvider<T> : ISpecificConfigurationProvider
    {
        /// <summary>
        /// Get configuration information from json.
        /// </summary>
        /// <param name="config">JSON data.</param>
        /// <returns>Parsed configuration information.</returns>
        T Get(JObject config);

        /// <summary>
        /// Update a configuration data.
        /// </summary>
        /// <param name="newValue">New configuration value to be used.</param>
        /// <param name="currentConfig">Current JSON data.</param>
        /// <returns>The updated JSON data.</returns>
        JObject Update(T newValue, JObject currentConfig);
    }
}
