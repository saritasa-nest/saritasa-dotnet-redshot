using System;
using System.Collections.Generic;
using System.Linq;
using RedShot.Infrastructure.Settings.Sections;
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
            SettingsSections = new List<Type>();
        }

        /// <summary>
        /// Settings options.
        /// Must implement ISettingsOption interface.
        /// </summary>
        public static List<Type> SettingsSections { get; }

        /// <summary>
        /// Open settings of the app.
        /// </summary>
        public static void OpenSettings()
        {
            if (settingsView != null && !settingsView.IsDisposed && settingsView.Loaded)
            {
                settingsView.BringToFront();
            }
            else
            {
                settingsView?.Close();

                settingsView = new SettingsView(ActivateSections());
                settingsView.Show();
            }
        }

        private static IEnumerable<ISettingsSection> ActivateSections()
        {
            return SettingsSections
                .Where(type => typeof(ISettingsSection).IsAssignableFrom(type) && !type.IsInterface)
                .Select(s => (ISettingsSection)Activator.CreateInstance(s)).ToList();
        }
    }
}
