namespace RedShot.Infrastructure.Abstractions.Configuration
{
    /// <summary>
    /// Configuration option.
    /// </summary>
    public interface IConfigurationOption
    {
        /// <summary>
        /// Unique name.
        /// </summary>
        string UniqueName { get; }
    }
}
