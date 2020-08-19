﻿namespace RedShot.Abstractions
{
    public interface IConfigurationSection
    {
        string UniqueName { get; }

        IConfigurationSection EncodeSection(IEncryptionService encryptionService);

        IConfigurationSection DecodeSection(IEncryptionService encryptionService);
    }
}
