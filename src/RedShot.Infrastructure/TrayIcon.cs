using System;
using Eto.Forms;
using Eto.Drawing;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// Tray icon for the app.
    /// </summary>
    public class TrayIcon : Form
    {
        /// <summary>
        /// Tray view.
        /// </summary>
        public TrayIndicator Tray { get; }

        public ButtonMenuItem UploadLastFile { get; }

        /// <summary>
        /// Inits tray icon.
        /// </summary>
        public TrayIcon(string title, Bitmap image)
        {
            var menu = new ContextMenu();

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Record",
                Command = new Command((e, o) => ApplicationManager.RunRecording())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Capture",
                Command = new Command((e, o) => ApplicationManager.RunScreenShooting())
            });

            UploadLastFile = new ButtonMenuItem()
            {
                Text = "Upload last file",
                Visible = false,
                Command = new Command((e, o) => UploadManager.UploadLastFile())
            };

            menu.Items.Add(UploadLastFile);
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Command = new Command((e, o) => Exit())
            });

            Tray = new TrayIndicator
            {
                Menu = menu,
                Title = title,
                Image = image
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
