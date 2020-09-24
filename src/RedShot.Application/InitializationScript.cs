using System;
using System.Collections.Generic;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Formatting.Settings;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Settings;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploading.Settings;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings;
using RedShot.Shortcut;
using RedShot.Shortcut.Settings;

namespace RedShot.Application
{
    /// <summary>
    /// Class which contains all configuration options and settings of the app.
    /// </summary>
    internal static class InitializationScript
    {
        static InitializationScript()
        {
            ConfigurationOptions = new List<Type>()
            {
                typeof(FtpConfiguration),
                typeof(FFmpegConfiguration),
                typeof(FormatConfigurationOption),
                typeof(ShortcutConfiguration),
                typeof(UploadingConfiguration)
            };

            SettingsOptions = new List<Type>()
            {
                typeof(FtpSettingsSection),
                typeof(RecordingSettingsSection),
                typeof(FormatSettingsSection),
                typeof(ShortcutSettingsSection),
                typeof(UploadingSettingsSection)
            };
        }

        /// <summary>
        /// List of configuration options of the app.
        /// They should implement IConfigurationOption interface.
        /// </summary>
        public static IEnumerable<Type> ConfigurationOptions { get; private set; }

        /// <summary>
        /// List of setting options of the app.
        /// They should implement ISettingOption interface.
        /// </summary>
        public static IEnumerable<Type> SettingsOptions { get; private set; }
    }
}
