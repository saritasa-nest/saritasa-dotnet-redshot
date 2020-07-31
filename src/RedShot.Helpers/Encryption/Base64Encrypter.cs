using RedShot.Abstractions;
using System;
using System.Text;

namespace RedShot.Helpers.Encryption
{
    public class Base64Encrypter : IEncryptionService
    {
        public string Decrypt(string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string Encrypt(string value)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
