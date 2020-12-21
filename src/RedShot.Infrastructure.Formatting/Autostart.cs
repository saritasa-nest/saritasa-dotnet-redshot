using System;
using System.IO;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Class responsible for changing autostart settings.
    /// </summary>
    public class Autostart
    {
        private const string ProgramName = "RedShot";
        private const string RegistrySubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\";
        private const string RegistryNotFoundMessage = "Failed to enable autostart, registry subkey not found.";

        private string ExecutablePath => Path.Combine(Directory.GetCurrentDirectory(),
            $"{AppDomain.CurrentDomain.FriendlyName}.exe");

        /// <summary>
        /// Enable autostart of the application.
        /// </summary>
        public void EnableAutostart()
        {
#if _WINDOWS
            EnableAutostartInWindows();
#endif
        }

        /// <summary>
        /// Disable autostart of the application.
        /// </summary>
        public void DisableAutostart()
        {
#if _WINDOWS
            DisableAutostartInWindows();
#endif
        }

        private void EnableAutostartInWindows()
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

        private void DisableAutostartInWindows()
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