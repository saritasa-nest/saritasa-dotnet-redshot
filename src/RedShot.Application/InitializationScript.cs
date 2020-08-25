using System;
using System.Collections.Generic;
using RedShot.DebugLib;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Settings;

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
                typeof(FFmpegConfiguration)
            };

            SettingOptions = new List<Type>()
            {
                typeof(FtpSettingOption),
                typeof(TestSetting),
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
        public static IEnumerable<Type> SettingOptions { get; private set; }
    }
}
