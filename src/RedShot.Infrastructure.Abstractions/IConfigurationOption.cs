using System;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Provides option for configuration.
    /// All object which represent configuration should implement this interface.
    /// </summary>
    public interface IConfigurationOption : ICloneable
    {
        /// <summary>
        /// Unique name of the option.
        /// </summary>
        string UniqueName { get; }
    }
}
