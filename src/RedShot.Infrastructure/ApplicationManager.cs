using System;
using Eto.Forms;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Screenshooting;
using RedShot.Infrastructure.Uploading;
using RedShot.Resources;

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
        /// Get tray icon form.
        /// </summary>
        public static Form GetTrayApp()
        {
            try
            {
                trayIcon = new TrayIcon("RedShot", Icons.RedCircle);
                UploadingManager.UploadStarted += UploadingManagerUploadStarted;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error occurred in creating tray icon form.");
                throw;
            }
            return trayIcon;
        }

        /// <summary>
        /// Run screen shooting view.
        /// </summary>
        public static void RunScreenShooting()
        {
            ScreenshotManager.TakeScreenShot();
        }

        /// <summary>
        /// Run recording.
        /// </summary>
        public static void RunRecording()
        {
            RecordingManager.InitiateRecording();
        }

        private static void UploadingManagerUploadStarted(object sender, EventArgs e)
        {
            trayIcon.UploadLastFile.Visible = true;
        }
    }
}
