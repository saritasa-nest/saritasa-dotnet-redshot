using System;
using System.Linq;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Settings;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// Tray icon for the application.
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
                Text = "Capture",
                ToolTip = "Take a screenshot",
                Command = new Command((e, o) => ApplicationManager.RunScreenShooting())
            });
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Record",
                ToolTip = "Open view for video recording",
                Command = new Command((e, o) => ApplicationManager.RunRecording())
            });

            UploadLastFile = new ButtonMenuItem()
            {
                Text = "Upload last file",
                ToolTip = "Upload last created file",
                Visible = false,
                Command = new Command((e, o) => ApplicationManager.UploadLastFile())
            };

            menu.Items.Add(UploadLastFile);

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Send feedback",
                Command = new Command((e, o) => ApplicationManager.SendFeedBack())
            });

            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Settings",
                Command = new Command((e, o) => SettingsManager.OpenSettings())
            });

            menu.Items.Add(new SeparatorMenuItem());
            menu.Items.Add(new ButtonMenuItem()
            {
                Text = "Exit",
                Command = new Command((e, o) => Close())
            });

            Tray = new TrayIndicator
            {
                Menu = menu,
                Title = title,
                Image = image
            };
        }

        /// <inheritdoc/>
        protected override void OnShown(EventArgs e)
        {
            Visible = false;
            Tray.Show();
            Tray.Visible = true;
        }

        /// <inheritdoc/>
        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            Tray.Hide();
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Instance.Dispose();
        }
    }
}
