using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Settings;

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

        /// <summary>
        /// Button for uploading last file.
        /// </summary>
        public ButtonMenuItem UploadLastFile { get; }

        /// <summary>
        /// Initializes tray icon.
        /// </summary>
        public TrayIcon(string title, Bitmap image)
        {
            var menu = new ContextMenu();

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Record",
                ToolTip = "Opens view for video recording",
                Shortcut = Keys.Control | Keys.F3,
                Command = new Command((e, o) => ApplicationManager.RunRecording())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Capture",
                ToolTip = "Opens view for screen shooting",
                Shortcut = Keys.Control | Keys.F2,
                Command = new Command((e, o) => ApplicationManager.RunScreenShooting())
            });

            UploadLastFile = new ButtonMenuItem()
            {
                Text = "Upload last file",
                Visible = false,
                Command = new Command((e, o) => UploadingManager.UploadLastFile())
            };

            menu.Items.Add(UploadLastFile);

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Settings",
                Command = new Command((e, o) => SettingsManager.OpenSettings())
            });

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Shortcut = Keys.Control | Keys.F4,
                Command = new Command((e, o) => Exit())
            });

            Tray = new TrayIndicator
            {
                Menu = menu,
                Title = title,
                Image = image
            };
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            Visible = false;
            Tray.Show();
            Tray.Visible = true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            Tray.Hide();
        }

        private void Exit()
        {
            this.Close();
            Application.Instance.Quit();
        }
    }
}
