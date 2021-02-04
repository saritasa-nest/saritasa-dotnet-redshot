using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
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

        private readonly Dictionary<Type, object> configurationOptions;
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
            configurationOptions = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Get configuration option or default.
        /// </summary>
        public T GetOptionOrDefault<T>() where T : class, new()
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
        public bool TryGetOption<T>(out T option) where T : class
        {
            var optionType = typeof(T);
            option = null;

            if (configurationOptions.TryGetValue(optionType, out var value))
            {
                option = value as T;
            }
            else if (jsonRootObject.Value.TryGetValue(optionType.FullName, out var jToken))
            {
                option = jToken.ToObject<T>(jsonSerializer);

                if (option is IEncryptable encryptable)
                {
                    option = encryptable.Decrypt(encryptionService) as T;
                }

                configurationOptions.Add(optionType, option);
            }

            return option != null;
        }

        /// <summary>
        /// Set configuration option.
        /// </summary>
        public void SetOption<T>(T option) where T : class
        {
            var optionType = typeof(T);
            configurationOptions.Remove(optionType);
            configurationOptions.Add(optionType, option);
        }

        /// <summary>
        /// Save configuration options.
        /// </summary>
        public void Save()
        {
            foreach (var keyValuePair in configurationOptions)
            {
                jsonRootObject.Value.Remove(keyValuePair.Key.FullName);

                var option = keyValuePair.Value;

                if (option is IEncryptable encryptable)
                {
                    option = encryptable.Encrypt(encryptionService);
                }

                jsonRootObject.Value.Add(keyValuePair.Key.FullName, JObject.FromObject(option));
            }

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
