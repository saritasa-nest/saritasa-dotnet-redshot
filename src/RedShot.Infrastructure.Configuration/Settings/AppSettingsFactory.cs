using System.IO;
using Microsoft.Extensions.Configuration;

namespace RedShot.Infrastructure.Configuration.Settings
{
    /// <summary>
    /// Application settings factory.
    /// </summary>
    internal static class AppSettingsFactory
    {
        /// <summary>
        /// Get application settings.
        /// </summary>
        public static AppSettings GetAppSettings()
        {
            var jsonStream = new MemoryStream(Properties.Resources.Appsettings);

            var root = new ConfigurationBuilder()
                .AddJsonStream(jsonStream)
                .Build();

            return root.GetSection("AppSettings").Get<AppSettings>();
        }
    }
}
