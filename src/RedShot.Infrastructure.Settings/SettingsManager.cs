using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Settings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedShot.Infrastructure.Settings
{
    public static class SettingsManager
    {
        private static SettingsView settingsView;

        static SettingsManager()
        {
            SettingOptions = new List<Type>();
        }

        public static List<Type> SettingOptions { get; }

        public static void OpenSettings()
        {
            settingsView?.Close();

            var options = SettingOptions
                .Where(type => typeof(ISettingsOption).IsAssignableFrom(type) && !type.IsInterface)
                .Select(t => (ISettingsOption)Activator.CreateInstance(t));

            settingsView = new SettingsView(options);
            settingsView.Closed += (o, e) =>
            {
                foreach (var option in options)
                {
                    option.Save();
                }
            };
            settingsView.Show();
        }
    }
}
