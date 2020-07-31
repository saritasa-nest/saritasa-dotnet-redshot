namespace RedShot.Abstractions
{
    public interface IEncryptionService
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
