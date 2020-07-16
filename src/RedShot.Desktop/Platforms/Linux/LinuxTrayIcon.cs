﻿using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace RedShot.Desktop.Platforms.Linux
{
    /// <summary>
    /// This Implements a Linux GTK3 Tray Icon
    /// </summary>
    public class LinuxTrayIcon : Eto.Forms.Form
    {
        public TrayIndicator _tray;
        private bool _startup = true;

        //Everything has to be sent in on the constructor since things do not auto-refresh / update this is a limitation
        public LinuxTrayIcon(string TooTip, string IconPath, ContextMenu menu)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                ClientSize = new Size(200, 200);
                _tray = new TrayIndicator
                {
                    Image = Eto.Drawing.Icon.FromResource(IconPath.Replace("resm:", "")),
                    Menu = menu,
                    Title = ToolTip
                };

                _tray.Show();
                _tray.Visible = true;
            });
        }

        protected override void OnShown(EventArgs e)
        {
            if (_startup)
            {
                Visible = false;
            }
        }

        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);
            _tray.Hide();
        }

    }
}
