using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;
using RedShot.Infrastructure.Configuration.Models;

namespace RedShot.Infrastructure.Configuration
{
    /// <summary>
    /// User configuration.
    /// </summary>
    public class UserConfiguration
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Application settings.
        /// </summary>
        public AppSettings AppSettings { get; private set; }

        private readonly List<IConfigurationOption> configurationOptions;
        private IEncryptionService encryptionService;
        private readonly Lazy<JsonDocument> jsonDocument;

        /// <summary>
        /// Initialize configuration manager.
        /// </summary>
        public UserConfiguration(AppSettings appSettings)
        {
            AppSettings = appSettings;
            encryptionService = new Base64Encrypter();
            jsonDocument = new Lazy<JsonDocument>(GetJsonDocument);
        }

        private JsonDocument GetJsonDocument()
        {
            var json = GetJsonText();
            return JsonDocument.Parse(json);
        }

        /// <summary>
        /// Returns section by the type.
        /// </summary>
        public T GetOption<T>() where T : class, IConfigurationOption, new()
        {
            var option = Activator.CreateInstance<T>() as IConfigurationOption;
        }

        private bool TryGetJsonElement(string elementName, out JsonElement jsonElement)
        {
            foreach (var element in jsonDocument.Value.RootElement.EnumerateArray())
            {
                if (element.TryGetProperty(nameof(IConfigurationOption.UniqueName), out var value))
                {
                    if (value.GetString().Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        jsonElement = element;
                        return true;
                    }
                }
            }

            jsonElement = default;
            return false;
        }

        /// <summary>
        /// Sets option's object.
        /// </summary>
        public void SetOption(IConfigurationOption option)
        {
        }

        /// <summary>
        /// Saves configuration to the file.
        /// </summary>
        public void Save()
        {
        }

        private string GetJsonText()
        {
            var path = GetConfigurationPath();
            if (File.Exists(path))
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetConfigurationPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RedShot",
                "config.json");
        }
    }
}
