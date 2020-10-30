using System;
using System.Collections.Generic;
using RedShot.Shortcut;
using RedShot.Shortcut.Settings;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Formatting.Settings;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Settings;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings;

namespace RedShot.Initialization
{
    /// <summary>
    /// Class which contains all configuration options and settings of the application.
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
                typeof(ShortcutSettingsSection)
            };
        }

        /// <summary>
        /// List of configuration options of the application.
        /// They should implement IConfigurationOption interface.
        /// </summary>
        public static IEnumerable<Type> ConfigurationOptions { get; private set; }

        /// <summary>
        /// List of setting options of the application.
        /// They should implement ISettingOption interface.
        /// </summary>
        public static IEnumerable<Type> SettingsOptions { get; private set; }
    }
}
