using System;
using Eto.Forms;
using Eto.Drawing;

namespace RedShot.App
{
    /// <summary>
    /// Tray icon for the app.
    /// </summary>
    public class TrayIcon : Eto.Forms.Form
    {
        /// <summary>
        /// Tray view.
        /// </summary>
        public readonly TrayIndicator Tray;

        // Everything has to be sent in on the constructor since things do not auto-refresh / update this is a limitation.
        public TrayIcon(string title, string iconPath)
        {
            var menu = new ContextMenu();

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Capture",
                Command = new Command((e, o) => ApplicationManager.RunScreenShotEditorDrawing())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Command = new Command((e, o) => Exit())
            });

            Tray = new TrayIndicator
            {
                Menu = menu,
                Title = title,
                Image = new Bitmap(iconPath)
            };
        }

        protected override void OnShown(EventArgs e)
        {
            Visible = false;
            Tray.Show();
            Tray.Visible = true;
        }

        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            Tray.Hide();
        }

        private void Exit()
        {
            this.Close();
        }
    }
}
