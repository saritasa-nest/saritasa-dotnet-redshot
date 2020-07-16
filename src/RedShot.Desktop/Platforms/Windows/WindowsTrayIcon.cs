using System;
using System.Collections.Generic;
using System.Text;
using Eto.Forms;
using System.Linq;
using Eto.Drawing;
using Avalonia.Threading;

namespace RedShot.Desktop.Platforms.Windows
{
    public class WindowsTrayIcon : Eto.Forms.Form
    {
        public TrayIndicator _tray;
        private bool _startup = true;

        //Everything has to be sent in on the constructor since things do not auto-refresh / update this is a limitation
        public WindowsTrayIcon(string TooTip, string IconPath, Avalonia.Controls.ContextMenu _menu)
        {

            var ctxMnu = new Eto.Forms.ContextMenu();
            foreach (var x in _menu.Items.Cast<Avalonia.Controls.MenuItem>())
            {
                ButtonMenuItem bmi = new ButtonMenuItem();
                bmi.Text = x.Header.ToString();
                bmi.Command = new Command((s, e) => {
                    Dispatcher.UIThread.Post(() =>
                    {
                        x.Command.Execute(null);
                    });
                });
                ctxMnu.Items.Add(bmi);
            }

            ClientSize = new Size(200, 200);
            _tray = new TrayIndicator
            {
                Image = Eto.Drawing.Icon.FromResource(IconPath.Replace("resm:", "")),
                Menu = ctxMnu,
                Title = ToolTip
            };

            _tray.Show();
            _tray.Visible = true;
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


