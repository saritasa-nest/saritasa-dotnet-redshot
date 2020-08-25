namespace RedShot.Infrastructure.Abstractions
{
    public interface IEncryptable
    {
        IEncryptable Encrypt(IEncryptionService encryptionService);

        IEncryptable Decrypt(IEncryptionService encryptionService);
    }
}
