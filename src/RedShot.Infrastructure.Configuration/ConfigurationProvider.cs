using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;
using RedShot.Infrastructure.Configuration.Models;
using RedShot.Infrastructure.Configuration.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Configuration
{
    /// <summary>
    /// Provides access to entire application configuration.
    /// </summary>
    public class ConfigurationProvider
    {
        private IDictionary<Type, IConfigurationProvider> configurationProviders;
        private readonly Lazy<JObject> jsonRootObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfigurationProvider()
        {
            jsonRootObject = new Lazy<JObject>(GetRootConfiguration);

            // TODO: inject the encryption service
            IEncryptionService encryptionService = new Base64Encrypter();
            RegisterConfigurationProviders(encryptionService);
        }

        /// <summary>
        /// The provider instance.
        /// </summary>
        // TODO: use dependency injection
        public static readonly ConfigurationProvider Instance = new ConfigurationProvider();

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

        private IConfigurationProvider<T> GetConfigurationProvider<T>()
        {
            if (configurationProviders.TryGetValue(typeof(T), out var provider))
            {
                return provider as IConfigurationProvider<T>;
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

            configurationProviders = new Dictionary<Type, IConfigurationProvider>()
            {
                { typeof(AccountConfiguration), new AccountConfigurationProvider(serializer) }
            };
        }
    }
}
