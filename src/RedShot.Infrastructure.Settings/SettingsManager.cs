using RedShot.Infrastructure.Settings.Views;

namespace RedShot.Infrastructure.Settings
{
    /// <summary>
    /// Settings manager.
    /// </summary>
    public static class SettingsManager
    {
        private static SettingsView settingsView;

        /// <summary>
        /// Open settings of the application.
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

                settingsView = new SettingsView();
                settingsView.Show();
            }
        }
    }
}
