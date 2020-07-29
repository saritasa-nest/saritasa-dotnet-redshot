using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RedShot.Configuration
{
    public static class ConfigurationManager
    {
        private static readonly string defaultFolderName = "RedShot";
        private static readonly string configName = "config.yaml";

        public static YamlConfig YamlConfig { get; } = GetConfig();

        public static void Save()
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(YamlConfig);

            var fullpath = GetFullPath();

            File.WriteAllText(fullpath, yaml);
        }

        private static YamlConfig GetConfig()
        {
            using (var reader = new StringReader(GetYamlString()))
            {
                var deserializer = new DeserializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();

                return deserializer.Deserialize<YamlConfig>(reader);
            }
        }

        private static string GetYamlString()
        {
            var fullpath = GetFullPath();

            if (File.Exists(fullpath))
            {
                return File.ReadAllText(fullpath);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetFullPath()
        {
            var folderPath = GetDefaultFolder();
            return Path.Combine(folderPath, configName);
        }

        private static string GetDefaultFolder()
        {
            return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), defaultFolderName)).FullName;
        }
    }
}
