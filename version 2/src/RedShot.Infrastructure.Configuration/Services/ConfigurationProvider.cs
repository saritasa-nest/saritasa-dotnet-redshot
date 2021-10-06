using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Configuration.Providers;
using RedShot.Infrastructure.Configuration.Models.Account;
using RedShot.Infrastructure.Configuration.Models.General;
using RedShot.Infrastructure.Configuration.Models.Recording;
using RedShot.Infrastructure.Configuration.Models.Shortcut;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Configuration.Json;

namespace RedShot.Infrastructure.Configuration.Services
{
    /// <summary>
    /// Provides access to entire application configuration.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private IDictionary<Type, ISpecificConfigurationProvider> configurationProviders;
        private readonly Lazy<JObject> jsonRootObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfigurationProvider(IEncryptionService encryptionService)
        {
            jsonRootObject = new Lazy<JObject>(GetRootConfiguration);
            RegisterConfigurationProviders(encryptionService);
        }

        /// <summary>
        /// Get a specific configuration information.
        /// </summary>
        /// <typeparam name="T">Type of configuration to get.</typeparam>
        /// <returns>Parsed configuration data.</returns>
        public T GetConfiguration<T>()
        {
            var provider = GetConfigurationProvider<T>();
            return provider.Get(jsonRootObject.Value);
        }

        /// <summary>
        /// Set a configuration overriding the current value.
        /// </summary>
        /// <typeparam name="T">Type of the configuration part to set.</typeparam>
        /// <param name="configurationParameter">The config object to use.</param>
        public void SetConfiguration<T>(T configurationParameter)
        {
            var provider = GetConfigurationProvider<T>();
            provider.Update(configurationParameter, jsonRootObject.Value);
        }

        private ISpecificConfigurationProvider<T> GetConfigurationProvider<T>()
        {
            if (configurationProviders.TryGetValue(typeof(T), out var provider))
            {
                return provider as ISpecificConfigurationProvider<T>;
            }

            throw new ArgumentException($"Cannot find a provider for specified configuration: {typeof(T).Name}");
        }

        /// <summary>
        /// Save the configuration to a file.
        /// </summary>
        public void Save()
        {
            var json = jsonRootObject.Value.ToString();
            var path = GetConfigurationPath();

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        private JObject GetRootConfiguration()
        {
            var path = GetConfigurationPath();
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                return JObject.Parse(json);
            }

            return new JObject();
        }

        private string GetConfigurationPath()
        {
            var directoryPath = Directory.CreateDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RedShot")).FullName;

            return Path.Combine(directoryPath, "config.json");
        }

        private void RegisterConfigurationProviders(IEncryptionService encryptionService)
        {
            var serializer = new JsonSerializer();
            serializer.Error += (o, e) => e.ErrorContext.Handled = true;
            serializer.ContractResolver = new EncryptedStringPropertyResolver(encryptionService);

            configurationProviders = new Dictionary<Type, ISpecificConfigurationProvider>()
            {
                { typeof(AccountConfiguration), new AccountConfigurationProvider(serializer) },
                { typeof(GeneralConfiguration), new GeneralConfigurationProvider(serializer) },
                { typeof(RecordingConfiguration), new RecordingConfigurationProvider(serializer) },
                { typeof(ShortcutConfiguration), new ShortcutConfigurationProvider(serializer) }
            };
        }
    }
}
