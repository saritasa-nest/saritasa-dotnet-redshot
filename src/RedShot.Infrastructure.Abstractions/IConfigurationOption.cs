using System;

namespace RedShot.Infrastructure.Abstractions
{
    public interface IConfigurationOption : ICloneable
    {
        string UniqueName { get; }

        IConfigurationOption EncodeSection(IEncryptionService encryptionService);

        IConfigurationOption DecodeSection(IEncryptionService encryptionService);
    }
}
