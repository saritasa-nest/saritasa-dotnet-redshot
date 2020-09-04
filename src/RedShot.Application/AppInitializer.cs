﻿using System;
using System.Linq;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Settings;
using RedShot.Infrastructure.Settings.Sections;

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
                .Where(type => typeof(ISettingsSection).IsAssignableFrom(type) && !type.IsInterface)
                .Select(t => (ISettingsSection)Activator.CreateInstance(t));

            SettingsManager.SettingsSections.AddRange(settingsOptions);
        }
    }
}
