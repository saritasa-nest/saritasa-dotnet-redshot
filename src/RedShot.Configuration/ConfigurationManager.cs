using System;
using System.IO;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;
using RedShot.Helpers.Encryption;

namespace RedShot.Configuration
{
    /// <summary>
    /// Manages configuration of the app.
    /// </summary>
    public static class ConfigurationManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string defaultFolderName = "RedShot";
        private static readonly string configName = "config.yaml";

        /// <summary>
        /// Gives configuration object.
        /// </summary>
        public static YamlConfig YamlConfig { get; } = GetConfig();

        /// <summary>
        /// Saves config in the file.
        /// </summary>
        public static void Save()
        {
            var encrypted = EncryptFtpAccountPasswords(YamlConfig);

            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(encrypted);

            var fullpath = GetFullPath();

            try
            {
                File.WriteAllText(fullpath, yaml);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private static YamlConfig GetConfig()
        {
            if (TryGetYamlString(out var conf))
            {
                using (var reader = new StringReader(conf))
                {
                    var deserializer = new DeserializerBuilder()
                        .IgnoreUnmatchedProperties()
                        .Build();

                    var config = deserializer.Deserialize<YamlConfig>(reader);

                    var decrypted = DecryptFtpAccountPasswords(config);

                    return decrypted;
                }
            }
            else
            {
                Logger.Info("There is not a config file in the system");
                return new YamlConfig();
            }
        }

        private static bool TryGetYamlString(out string conf)
        {
            var fullpath = GetFullPath();

            if (File.Exists(fullpath))
            {
                try
                {
                    conf = File.ReadAllText(fullpath);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error occured in reading config file");
                    throw;
                }
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
            return Path.Combine(folderPath, configName);
        }

        private static string GetDefaultFolder()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), defaultFolderName)).FullName;
                }
                else
                {
                    return Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), defaultFolderName)).FullName;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occured in creating RedShot config folder");
                throw;
            }
        }

        private static YamlConfig EncryptFtpAccountPasswords(YamlConfig config)
        {
            var clone = config.Clone();

            var encryptService = new Base64Encrypter();

            foreach (var account in clone.FtpAccounts)
            {
                account.Password = encryptService.Encrypt(account.Password);

                account.Passphrase = encryptService.Encrypt(account.Passphrase);
            }

            return clone;
        }

        private static YamlConfig DecryptFtpAccountPasswords(YamlConfig config)
        {
            var clone = config.Clone();

            var encryptService = new Base64Encrypter();

            foreach (var account in clone.FtpAccounts)
            {
                try
                {
                    account.Password = encryptService.Decrypt(account.Password);
                    account.Passphrase = encryptService.Decrypt(account.Passphrase);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex);
                    // Occures when passwords are null.
                }
            }

            return clone;
        }
    }
}
