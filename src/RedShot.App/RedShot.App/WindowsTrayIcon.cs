﻿using System;
using Eto.Forms;
using Eto.Drawing;

namespace RedShot.App
{
    public class WindowsTrayIcon : Eto.Forms.Form
    {
        public readonly TrayIndicator tray;

        // Everything has to be sent in on the constructor since things do not auto-refresh / update this is a limitation.
        public WindowsTrayIcon(string title, string iconPath)
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

            tray = new TrayIndicator
            {
                Menu = menu,
                Title = title,
                Image = new Bitmap(iconPath)
            };
        }

        protected override void OnShown(EventArgs e)
        {
            Visible = false;
            tray.Show();
            tray.Visible = true;
        }

        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            tray.Hide();
        }

        private void Exit()
        {
            this.Close();
        }
    }
}
