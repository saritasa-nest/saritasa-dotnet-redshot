using RedShot.Abstractions;
using System;
using System.Text;

namespace RedShot.Helpers.Encryption
{
    /// <summary>
    /// Base 64 implementation of encryption service.
    /// </summary>
    public class Base64Encrypter : IEncryptionService
    {
        /// <summary>
        /// Decrypt value.
        /// </summary>
        public string Decrypt(string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Encrypt value.
        /// </summary>
        public string Encrypt(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
