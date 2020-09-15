using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Recording;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// Manages application views.
    /// </summary>
    public static class ApplicationManager
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static TrayIcon trayIcon;

        /// <summary>
        /// Gives tray icon form.
        /// </summary>
        /// <returns>Form.</returns>
        public static Form GetTrayApp()
        {
            try
            {
                trayIcon = new TrayIcon("RedShot", new Bitmap(Resources.Properties.Resources.Redcircle));
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error occurred in creating tray icon form.");
                throw;
            }
            return trayIcon;
        }

        /// <summary>
        /// Runs screenshot editor.
        /// </summary>
        public static void RunScreenShooting()
        {
            ScreenshotManager.RunScreenShotting();
        }

        /// <summary>
        /// Runs upload view.
        /// </summary>
        public static void RunUploadView(IFile file)
        {
            trayIcon.UploadLastFile.Visible = true;
            UploadingManager.RunUploading(file);
        }

        /// <summary>
        /// Runs recording.
        /// </summary>
        public static void RunRecording()
        {
            trayIcon.UploadLastFile.Visible = true;
            RecordingManager.InitiateRecording();
        }
    }
}
