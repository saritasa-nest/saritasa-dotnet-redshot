using System.Collections.Generic;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Settings.Views;

namespace RedShot.Infrastructure.Settings
{
    /// <summary>
    /// Settings manager.
    /// </summary>
    public static class SettingsManager
    {
        private static SettingsView settingsView;

        static SettingsManager()
        {
            SettingsOptions = new List<ISettingsOption>();
        }

        /// <summary>
        /// Settings options.
        /// Must implement ISettingsOption interface.
        /// </summary>
        public static List<ISettingsOption> SettingsOptions { get; }

        /// <summary>
        /// Open settings of the app.
        /// </summary>
        public static void OpenSettings()
        {
            settingsView?.Close();

            settingsView = new SettingsView(SettingsOptions);
            settingsView.Show();
        }
    }
}
