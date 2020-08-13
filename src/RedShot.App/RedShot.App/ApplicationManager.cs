using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.App.Painting;
using RedShot.Recording;
using RedShot.Upload.Forms;

namespace RedShot.App
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
        private static EditorViewDrawingSkiaSharp editorView;

        /// <summary>
        /// Gives tray icon form.
        /// </summary>
        /// <returns></returns>
        public static Form GetTrayApp()
        {
            try
            {
                trayIcon = new TrayIcon("RedShot", new Bitmap(Properties.Resources.red_circle));
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
        public static void RunScreenShotEditorDrawing()
        {
            try
            {
                trayIcon.Tray.Visible = false;

                CloseViews();

                editorView = new EditorViewDrawingSkiaSharp();
                editorView.Closed += View_Closed;
                editorView.Show();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Error occurred in creating tray screen editor form");
                throw;
            }
        }

        public static void RunPaintingView(Bitmap bitmap)
        {
            CloseViews();
            paintingView = new PaintingView(bitmap);
            paintingView.Show();
        }

        public static void RunUploadView(Bitmap bitmap)
        {
            CloseViews();
            uploadBar = new UploadBar(bitmap);
            uploadBar.Show();
        }

        public static void RunRecodringView()
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
