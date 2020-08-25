using System;
using System.Text;
using RedShot.Infrastructure.Abstractions;

namespace RedShot.Infrastructure.Common.Encryption
{
    /// <summary>
    /// Base 64 implementation of encryption service. In general it doesn't provide any
    /// security, the purpose is to no store plain text passwords.
    /// </summary>
    public class Base64Encrypter : IEncryptionService
    {
        /// <inheritdoc />
        public string Decrypt(string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <inheritdoc />
        public string Encrypt(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
