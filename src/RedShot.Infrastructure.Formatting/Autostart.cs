using System;
using System.IO;
using System.Reflection;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Class responsible for changing autostart settings.
    /// </summary>
    public class Autostart
    {
        private const string ProgramName = "RedShot";
        private const string RegistrySubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\";
        private const string RegistryNotFoundMessage = "Faild to enable autostart, registry subkey not found";

        private bool previousAutostartOption;

        private string ExecutablePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            $"{AppDomain.CurrentDomain.FriendlyName}.exe");

        /// <summary>
        /// Initializes <see cref="Autostart"/> object.
        /// </summary>
        /// <param name="generalConfiguration">General configuration object.</param>
        public Autostart(GeneralConfigurationOption generalConfiguration)
        {
            previousAutostartOption = generalConfiguration.LaunchAtSystemStart.Value;
        }

        /// <summary>
        /// Whether to launch at system start.
        /// </summary>
        public bool LaunchAtSystemStart
        {
            set
            {
                if (previousAutostartOption == value)
                {
                    return;
                }

                if (value)
                {
                    EnableAutostart();
                }
                else
                {
                    DisableAutostart();
                }
            }
        }

        private void EnableAutostart()
        {
            var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistrySubKey, true);
            try
            {
                key.SetValue(ProgramName, ExecutablePath);
                key.Close();
            }
            catch (NullReferenceException)
            {
                throw new InvalidOperationException(RegistryNotFoundMessage);
            }
        }

        private void DisableAutostart()
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistrySubKey, true);

            if (key == null)
            {
                return;
            }

            try
            {
                key.DeleteValue(ProgramName, false);
                key.Close();
            }
            catch (NullReferenceException)
            {
                throw new InvalidOperationException(RegistryNotFoundMessage);
            }
        }
    }
}