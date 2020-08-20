using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;
using RedShot.Infrastructure.DataTransfer;

namespace RedShot.Infrastructure.Configuration
{
    public static class ConfigurationManager
    {
        private const string DefaultFolderName = "RedShot";
        private const string ConfigName = "config.json";
        private static readonly NLog.Logger logger;
        private static readonly Dictionary<Type, IConfigurationOption> settingsMap;
        private static readonly IEncryptionService encryptionService;

        static ConfigurationManager()
        {
            encryptionService = new Base64Encrypter();
            logger = NLog.LogManager.GetCurrentClassLogger();
            settingsMap = new Dictionary<Type, IConfigurationOption>();

            Load();
        }

        public static T GetSection<T>() where T : class, IConfigurationOption, new()
        {
            var type = typeof(T);

            if (settingsMap.TryGetValue(type, out var section))
            {
                return (T)section.Clone();
            }
            else
            {
                throw new KeyNotFoundException($"{type.Name} section isn't defined in RedShot.Infrastructure.DataTransfer project. Please, do it.");
            }
        }

        public static void SetSettingsValue(IConfigurationOption section)
        {
            var sectionType = section.GetType();

            if (settingsMap.ContainsKey(sectionType))
            {
                settingsMap.Remove(sectionType);
            }

            settingsMap.Add(sectionType, section);
        }

        public static void Save()
        {
            var rootObject = new JObject();

            foreach (var mapPair in settingsMap)
            {
                rootObject.Add(mapPair.Key.Name, JObject.FromObject(mapPair.Value.EncodeSection(encryptionService)));
            }

            var settingsFilePath = GetFullPath();

            File.WriteAllText(settingsFilePath, rootObject.ToString(Formatting.Indented));
        }

        private static void Load()
        {
            var types = Assembly
                .GetAssembly(typeof(ConfigurationManager))
                ?.GetTypes()
                .Where(type => typeof(IConfigurationOption).IsAssignableFrom(type) && !type.IsInterface);

            var settingsFile = new JObject();

            if (TryGetConfigString(out var config))
            {
                settingsFile = JObject.Parse(config);
            }

            foreach (var type in types)
            {
                if (settingsFile.ContainsKey(type.Name) && settingsFile[type.Name] is JObject section)
                {
                    settingsMap.Add(type, (section.ToObject(type) as IConfigurationOption).DecodeSection(encryptionService));
                }
                else
                {
                    settingsMap.Add(type, (IConfigurationOption)Activator.CreateInstance(type));
                }
            }
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DefaultFolderName)).FullName;
            }
            else
            {
                return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DefaultFolderName)).FullName;
            }
        }
    }
}
