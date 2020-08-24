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

#if DEBUG
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Test",
                Command = new Command((e, o) => RedShot.DebugLib.TestClass.RunTest())
            });
#endif

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Record",
                ToolTip = "Opens view for video recording",
                Command = new Command((e, o) => ApplicationManager.RunRecording())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Capture",
                ToolTip = "Opens view for screen shooting",
                Command = new Command((e, o) => ApplicationManager.RunScreenShooting())
            });

            UploadLastFile = new ButtonMenuItem()
            {
                Text = "Upload last file",
                Visible = false,
                Command = new Command((e, o) => UploadManager.UploadLastFile())
            };

            menu.Items.Add(UploadLastFile);
            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Shortcut = Application.Instance.AlternateModifier | Keys.F4,
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
            Application.Instance.Quit();
        }
    }
}
