using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings;

namespace RedShot.Application
{
    internal static class AppInitializer
    {
        /// <summary>
        /// Initializes configuration options and settings.
        /// </summary>
        public static void Initialize()
        {
            ConfigurationManager.ConfigurationOptions.AddRange(InitializationScript.ConfigurationOptions);
            ConfigurationManager.Load();

            SettingsManager.SettingOptions.AddRange(InitializationScript.SettingOptions);
        }
    }
}
