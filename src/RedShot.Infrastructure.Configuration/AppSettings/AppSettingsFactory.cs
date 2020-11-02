using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace RedShot.Infrastructure.Configuration.AppSettings
{
    /// <summary>
    /// Application settings factory.
    /// </summary>
    internal static class AppSettingsFactory
    {
        private const string RedShotBinaryFolderVariable = "NLOG__VARIABLES__RedShotBinaryFolder";

        /// <summary>
        /// Get application settings.
        /// </summary>
        public static IConfigurationRoot GetAppSettings()
        {
            var applicationFolder = Directory.GetCurrentDirectory();
            Environment.SetEnvironmentVariable(RedShotBinaryFolderVariable, applicationFolder);

            return new ConfigurationBuilder()
                .SetBasePath(applicationFolder)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
