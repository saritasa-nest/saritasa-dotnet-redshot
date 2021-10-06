namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IConfigurationProvider
    {
        // <summary>
        /// Get a specific configuration information.
        /// </summary>
        /// <typeparam name="T">Type of configuration to get.</typeparam>
        /// <returns>Parsed configuration data.</returns>
        public T GetConfiguration<T>();

        /// <summary>
        /// Set a configuration overriding the current value.
        /// </summary>
        /// <typeparam name="T">Type of the configuration part to set.</typeparam>
        /// <param name="configurationParameter">The config object to use.</param>
        public void SetConfiguration<T>(T configurationParameter);

        /// <summary>
        /// Save the configuration to a file.
        /// </summary>
        public void Save();
    }
}
