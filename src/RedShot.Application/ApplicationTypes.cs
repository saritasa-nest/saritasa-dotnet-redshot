using System;
using System.Collections.Generic;
using RedShot.Shortcut;
using RedShot.Shortcut.Settings;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Formatting.Settings;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Settings;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploading.Settings;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings;

namespace RedShot.Application
{
    /// <summary>
    /// Class which contains all options and settings types of the application.
    /// </summary>
    internal class ApplicationTypes
    {
        /// <summary>
        /// Create application types.
        /// </summary>
        public ApplicationTypes()
        {
            ConfigurationOptionsTypes = new List<Type>()
            {
                typeof(FtpConfiguration),
                typeof(FFmpegConfiguration),
                typeof(FormatConfigurationOption),
                typeof(ShortcutConfiguration),
                typeof(UploadingConfiguration)
            };

            SettingsOptionsTypes = new List<Type>()
            {
                typeof(FtpSettingsSection),
                typeof(RecordingSettingsSection),
                typeof(FormatSettingsSection),
                typeof(ShortcutSettingsSection),
                typeof(UploadingSettingsSection)
            };
        }

        /// <summary>
        /// List of configuration options of the application.
        /// They should implement IConfigurationOption interface.
        /// </summary>
        public IEnumerable<Type> ConfigurationOptionsTypes { get; }

        /// <summary>
        /// List of setting options of the application.
        /// They should implement ISettingOption interface.
        /// </summary>
        public IEnumerable<Type> SettingsOptionsTypes { get; }
    }
}
