using Eto.Drawing;

namespace RedShot.Platforms.Linux
{
    public static class GtkHelper
    {
        public static void SetFullScreen(object control, Size size)
        {
            if (control is Gtk.Window window)
            {
                window.SetSizeRequest(size.Width, size.Height);
                window.Fullscreen();
            }
        }
    }
}
