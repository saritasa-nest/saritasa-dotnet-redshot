using Eto.Drawing;
using Eto.Forms;
using RedShot.Upload.Forms;
using System.IO;

namespace RedShot.App
{
    public static class ApplicationManager
    {
        private static TrayIcon trayIcon;

        public static Form GetTrayApp()
        {
            trayIcon = new TrayIcon("RedShot", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "red-circle.png"));
            return trayIcon;
        }

        public static void RunScreenShotEditorDrawing()
        {
            trayIcon.Tray.Visible = false;

            var view = new EditorViewDrawingSkiaSharp();
            view.Closed += View_Closed;
            view.Show();
        }

        public static void RunUploaderView(Bitmap image)
        {
            var view = new UploadBar(image);
            view.Show();
        }

        private static void View_Closed(object sender, System.EventArgs e)
        {
            trayIcon.Tray.Visible = true;
        }
    }
}
