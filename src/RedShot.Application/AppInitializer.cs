using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings;

namespace RedShot.Application
{
    /// <summary>
    /// Application initializer.
    /// </summary>
    internal static class AppInitializer
    {
        /// <summary>
        /// Initializes configuration options and settings.
        /// </summary>
        public static void Initialize()
        {
            ConfigurationManager.ConfigurationOptions.AddRange(InitializationScript.ConfigurationOptions);
            ConfigurationManager.Load();

            SettingsManager.SettingsSections.AddRange(InitializationScript.SettingsOptions);
        }
    }
}
