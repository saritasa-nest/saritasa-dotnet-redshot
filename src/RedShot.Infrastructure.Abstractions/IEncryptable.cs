namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Provides encryption methods for the object.
    /// Uses in configuration for hiding private information (passwords, keys).
    /// </summary>
    public interface IEncryptable
    {
        /// <summary>
        /// Encrypt object via encryption service.
        /// </summary>
        IEncryptable Encrypt(IEncryptionService encryptionService);

        /// <summary>
        /// Decrypt object via encryption service.
        /// </summary>
        IEncryptable Decrypt(IEncryptionService encryptionService);
    }
}
