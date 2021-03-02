using System;
using System.Collections.Generic;
using RedShot.Shortcut.Settings;
using RedShot.Infrastructure.Formatting.Settings;
using RedShot.Infrastructure.Recording.Settings;
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
            SettingsOptionsTypes = new List<Type>()
            {
                typeof(RecordingSettingsSection),
                typeof(GeneralSettingsSection),
                typeof(ShortcutSettingsSection)
            };
        }

        /// <summary>
        /// List of setting options of the application.
        /// They should implement ISettingOption interface.
        /// </summary>
        public IEnumerable<Type> SettingsOptionsTypes { get; }
    }
}
