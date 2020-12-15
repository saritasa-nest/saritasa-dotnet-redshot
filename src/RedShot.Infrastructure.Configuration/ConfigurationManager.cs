﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;
using RedShot.Infrastructure.Configuration.Models;

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
        public static IEnumerable<Type> ConfigurationOptions { get; private set; }

        /// <summary>
        /// Application settings.
        /// </summary>
        public static AppSettings AppSettings { get; private set; }

        private const string DefaultFolderName = "RedShot";
        private const string ConfigName = "config.json";
        private static NLog.Logger logger;
        private static Dictionary<Type, IConfigurationOption> settingsMap;
        private static IEncryptionService encryptionService;

        /// <summary>
        /// Initialize configuration manager.
        /// </summary>
        public static void Initialize(AppSettings appSettings, IEnumerable<Type> configurationOptions)
        {
            AppSettings = appSettings;
            ConfigurationOptions = configurationOptions;
            encryptionService = new Base64Encrypter();
            logger = NLog.LogManager.GetCurrentClassLogger();
            settingsMap = new Dictionary<Type, IConfigurationOption>();

            Load();
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
        public static void SetSection(IConfigurationOption option)
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
        private static void Load()
        {
            var types = ConfigurationOptions.Where(type => typeof(IConfigurationOption).IsAssignableFrom(type) && !type.IsInterface);

            var settingsFile = new JObject();

            if (TryGetConfigString(out var config))
            {
                settingsFile = JObject.Parse(config);
            }

            foreach (var type in types)
            {
                var configuration = GetConfiguration(settingsFile, type);
                settingsMap.Add(type, configuration);
            }

            logger.Debug("The configuration has been loaded.");
        }

        private static IConfigurationOption GetConfiguration(JObject settings, Type configurationType)
        {
            if (settings.ContainsKey(configurationType.Name) && settings[configurationType.Name] is JObject section)
            {
                object configurationObject;
                try
                {
                    configurationObject = section.ToObject(configurationType);

                    if (configurationObject is IEncryptable encryptable)
                    {
                        configurationObject = encryptable.Decrypt(encryptionService);
                    }
                    return configurationObject as IConfigurationOption;
                }
                catch (JsonSerializationException ex)
                {
                    logger.Error(ex, "Could not restore configuration for {settingName}. Removing from the settings file.", configurationType.Name);
                    // This can happen if setting does no longer exist
                    // Just remove the setting
                    settings.Remove(configurationType.Name);
                }
            }

            // The configuration was not found or there was an error parsing it from the config.
            return (IConfigurationOption)Activator.CreateInstance(configurationType);
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
