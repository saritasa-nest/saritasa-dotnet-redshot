using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;
using RedShot.Infrastructure.Configuration.AppSettings;

namespace RedShot.Infrastructure.Configuration
{
    /// <summary>
    /// Configuration manager.
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// Configuration options.
        /// Must implement IConfigurationOption interface.
        /// </summary>
        public static List<Type> ConfigurationOptions { get; }

        /// <summary>
        /// Application settings.
        /// </summary>
        public static IConfigurationRoot AppSettings { get; }

        private const string DefaultFolderName = "RedShot";
        private const string ConfigName = "config.json";
        private static readonly NLog.Logger logger;
        private static readonly Dictionary<Type, IConfigurationOption> settingsMap;
        private static readonly IEncryptionService encryptionService;

        static ConfigurationManager()
        {
            AppSettings = AppSettingsFactory.GetAppSettings();

            ConfigurationOptions = new List<Type>();
            encryptionService = new Base64Encrypter();
            logger = NLog.LogManager.GetCurrentClassLogger();
            settingsMap = new Dictionary<Type, IConfigurationOption>();
        }

        /// <summary>
        /// Returns section by the type.
        /// </summary>
        public static T GetSection<T>() where T : class, IConfigurationOption, new()
        {
            var type = typeof(T);

            if (settingsMap.TryGetValue(type, out var section))
            {
                return (T)section.Clone();
            }
            else
            {
                throw new KeyNotFoundException($"{type.Name} section isn't defined. Please, do it.");
            }
        }

        /// <summary>
        /// Sets option's object.
        /// </summary>
        public static void SetSettingsValue(IConfigurationOption option)
        {
            var optionType = option.GetType();

            if (settingsMap.ContainsKey(optionType))
            {
                settingsMap.Remove(optionType);
            }

            settingsMap.Add(optionType, option);
        }

        /// <summary>
        /// Saves configuration to the file.
        /// </summary>
        public static void Save()
        {
            var rootObject = new JObject();

            foreach (var mapPair in settingsMap)
            {
                var configurationObject = mapPair.Value;

                if (configurationObject is IEncryptable encryptable)
                {
                    configurationObject = (IConfigurationOption)encryptable.Encrypt(encryptionService);
                }

                rootObject.Add(mapPair.Key.Name, JObject.FromObject(configurationObject));
            }

            var settingsFilePath = GetFullPath();
            File.WriteAllText(settingsFilePath, rootObject.ToString(Formatting.Indented));
            logger.Debug("The configuration has been saved.");
        }

        /// <summary>
        /// Loads data from the file.
        /// </summary>
        public static void Load()
        {
            var types = ConfigurationOptions.Where(type => typeof(IConfigurationOption).IsAssignableFrom(type) && !type.IsInterface);

            var settingsFile = new JObject();

            if (TryGetConfigString(out var config))
            {
                settingsFile = JObject.Parse(config);
            }

            foreach (var type in types)
            {
                if (settingsFile.ContainsKey(type.Name) && settingsFile[type.Name] is JObject section)
                {
                    var configurationObject = section.ToObject(type);

                    if (configurationObject is IEncryptable encryptable)
                    {
                        configurationObject = encryptable.Decrypt(encryptionService);
                    }

                    settingsMap.Add(type, configurationObject as IConfigurationOption);
                }
                else
                {
                    settingsMap.Add(type, (IConfigurationOption)Activator.CreateInstance(type));
                }
            }
            logger.Debug("The configuration has been loaded.");
        }

        private static bool TryGetConfigString(out string conf)
        {
            var path = GetFullPath();

            if (File.Exists(path))
            {
                conf = File.ReadAllText(path);
                return true;
            }
            else
            {
                conf = string.Empty;
                return false;
            }
        }

        private static string GetFullPath()
        {
            var folderPath = GetDefaultFolder();
            return Path.Combine(folderPath, ConfigName);
        }

        private static string GetDefaultFolder()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DefaultFolderName);
            return Directory.CreateDirectory(path).FullName;
        }
    }
}
