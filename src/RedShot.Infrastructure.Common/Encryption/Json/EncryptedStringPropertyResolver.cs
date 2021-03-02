using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RedShot.Infrastructure.Common.Encryption
{
    /// <summary>
    /// Property resolver that is automatically encrypts and decrypts properties marked with <see cref="PropertyEncryptAttribute"/>.
    /// </summary>
    public class EncryptedStringPropertyResolver : DefaultContractResolver
    {
        private IEncryptionService encryptionService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EncryptedStringPropertyResolver(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);

            // Find all string properties that have a [JsonEncrypt] attribute applied
            // and attach an EncryptedStringValueProvider instance to them
            foreach (JsonProperty prop in props.Where(p => p.PropertyType == typeof(string)))
            {
                var pi = type.GetProperty(prop.UnderlyingName);
                if (pi != null && pi.GetCustomAttribute(typeof(PropertyEncryptAttribute), true) != null)
                {
                    prop.ValueProvider =
                        new EncryptedStringValueProvider(pi, encryptionService);
                }
            }

            return props;
        }

        /// <summary>
        /// Encrypts and decrypts a value of a property.
        /// </summary>
        class EncryptedStringValueProvider : IValueProvider
        {
            private PropertyInfo targetProperty;
            private IEncryptionService encryptionService;

            /// <summary>
            /// Constructor.
            /// </summary>
            public EncryptedStringValueProvider(PropertyInfo targetProperty, IEncryptionService encryptionService)
            {
                this.targetProperty = targetProperty;
                this.encryptionService = encryptionService;
            }

            /// <inheritdoc />
            public object GetValue(object target)
            {
                string value = (string)targetProperty.GetValue(target);

                return encryptionService.Encrypt(value);
            }

            /// <inheritdoc />
            public void SetValue(object target, object value)
            {
                var decrypted = encryptionService.Decrypt((string)value);
                targetProperty.SetValue(target, decrypted);
            }
        }
    }
}
