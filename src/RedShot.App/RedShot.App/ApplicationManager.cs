using Eto.Forms;
using System.IO;

namespace RedShot.App
{
    public static class ApplicationManager
    {
        private static Form TrayIcon;

        public static Form GetTrayApp()
        {
            TrayIcon = new WindowsTrayIcon("RedShot", Path.Combine(Directory.GetCurrentDirectory(), "Resources", "red-circle.png"));
            return TrayIcon;
        }

        public static void RunScreenShotRedactor()
        {
            TrayIcon.Visible = false;

            var view = new EditorView();
            view.Show();

            TrayIcon.Visible = true;
        }

        public static void RunScreenShotEditorDrawing()
        {
            TrayIcon.Visible = false;

            var view = new EditorViewDrawing();
            view.Show();

            TrayIcon.Visible = true;
        }
    }
}
