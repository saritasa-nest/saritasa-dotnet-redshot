using System;
using System.Linq;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings;

namespace RedShot.Application
{
    /// <summary>
    /// App initializer.
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

            var settingsOptions = InitializationScript.SettingsOptions
                .Where(type => typeof(ISettingsOption).IsAssignableFrom(type) && !type.IsInterface)
                .Select(t => (ISettingsOption)Activator.CreateInstance(t));

            SettingsManager.SettingsOptions.AddRange(settingsOptions);
        }
    }
}
