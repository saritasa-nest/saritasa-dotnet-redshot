using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Encryption;

namespace RedShot.Infrastructure.Configuration
{
    /// <summary>
    /// User configuration.
    /// </summary>
    public class UserConfiguration
    {
        /// <summary>
        /// Instance of user configuration.
        /// </summary>
        public static UserConfiguration Instance { get; } = new UserConfiguration();

        private IEncryptionService encryptionService;
        private readonly Lazy<JObject> jsonRootObject;
        private readonly JsonSerializer jsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserConfiguration()
        {
            encryptionService = new Base64Encrypter();
            jsonRootObject = new Lazy<JObject>(GetJsonRootObject);
            jsonSerializer = GetSafeJsonSerializer();
        }

        /// <summary>
        /// Get configuration option or default.
        /// </summary>
        public T GetOptionOrDefault<T>() where T : new()
        {
            if (TryGetOption<T>(out var value))
            {
                return value;
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// Try get the option.
        /// </summary>
        public bool TryGetOption<T>(out T option)
        {
            var optionType = typeof(T);
            option = default;

            if (jsonRootObject.Value.TryGetValue(optionType.FullName, out var jToken))
            {
                option = jToken.ToObject<T>(jsonSerializer);

                if (option is IEncryptable encryptable)
                {
                    option = (T)encryptable.Decrypt(encryptionService);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Set configuration option.
        /// </summary>
        public void SetOption<T>(T option)
        {
            var optionType = typeof(T);
            jsonRootObject.Value.Remove(optionType.FullName);

            if (option is IEncryptable encryptable)
            {
                option = (T)encryptable.Encrypt(encryptionService);
            }

            jsonRootObject.Value.Add(optionType.FullName, JObject.FromObject(option));
        }

        /// <summary>
        /// Save configuration options.
        /// </summary>
        public void Save()
        {
            var json = jsonRootObject.Value.ToString();
            var path = GetConfigurationPath();

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        private JObject GetJsonRootObject()
        {
            var json = GetJsonText();
            return JObject.Parse(json);
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
                return "{}";
            }
        }

        private string GetConfigurationPath()
        {
            var directoryPath = Directory.CreateDirectory(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RedShot")).FullName;

            return Path.Combine(directoryPath, "config.json");
        }

        /// <summary>
        /// Get a safe JSON serializer that doesn't throw exceptions when the configuration parameters changed.
        /// </summary>
        private JsonSerializer GetSafeJsonSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.Error += (o, e) => e.ErrorContext.Handled = true;

            return serializer;
        }
    }
}
