using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Forms;
using RedShot.Infrastructure.Painting;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Screenshooting;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// Manages application views.
    /// </summary>
    public static class ApplicationManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static TrayIcon trayIcon;
        private static UploadBar uploadBar;
        private static PaintingView paintingView;
        private static ScreenShotSelectionView editorView;

        /// <summary>
        /// Gives tray icon form.
        /// </summary>
        /// <returns></returns>
        public static Form GetTrayApp()
        {
            try
            {
                trayIcon = new TrayIcon("RedShot", new Bitmap(Resources.Properties.Resources.Redcircle));
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Error occurred in creating tray icon form");
                throw;
            }
            return trayIcon;
        }

        /// <summary>
        /// Runs screenshot editor.
        /// </summary>
        public static void RunScreenShooting()
        {
            trayIcon.Tray.Visible = false;

            ScreenshotManager.TakeScreenShot();

            trayIcon.Tray.Visible = true;
        }

        public static void RunUploadView(IFile file)
        {
            UploadManager.RunUploading(file);
        }

        public static void RunRecording()
        {
            RecordingManager.InitiateRecording();
        }

        private static void CloseViews()
        {
            if (editorView != null && !editorView.IsDisposed)
            {
                editorView.Close();
            }

            if (uploadBar != null && !uploadBar.IsDisposed)
            {
                uploadBar.Close();
            }

            if (paintingView != null && !paintingView.IsDisposed)
            {
                paintingView.Close();
            }
        }

        private static void View_Closed(object sender, System.EventArgs e)
        {
            trayIcon.Tray.Visible = true;
        }
    }
}
