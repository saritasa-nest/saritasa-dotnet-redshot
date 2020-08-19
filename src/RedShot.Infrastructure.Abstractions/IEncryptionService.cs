namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Encryption service abstraction.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Method for encrypting.
        /// </summary>
        string Encrypt(string value);

        /// <summary>
        /// Method for decrypting.
        /// </summary>
        string Decrypt(string value);
    }
}
