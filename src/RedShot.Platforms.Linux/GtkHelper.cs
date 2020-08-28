﻿using Eto.Drawing;

namespace RedShot.Platforms.Linux
{
    public static class GtkHelper
    {
        public static void SetFullScreen(object control, Size size)
        {
            if (control is Gtk.Widget widget)
            {
                widget.SetSizeRequest(size.Width, size.Height);
                widget.Window.Fullscreen();
            }
        }
    }
}
