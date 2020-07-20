using Eto.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
