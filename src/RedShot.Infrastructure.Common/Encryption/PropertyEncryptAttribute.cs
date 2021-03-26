using System;

namespace RedShot.Infrastructure.Common.Encryption
{
    /// <summary>
    /// Marks a property to be encrypted when serialized to JSON.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEncryptAttribute : Attribute
    {
    }
}
