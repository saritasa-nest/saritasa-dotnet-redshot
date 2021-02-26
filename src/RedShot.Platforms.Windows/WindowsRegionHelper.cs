using System.Drawing;
using System.Windows.Forms;

namespace RedShot.Platforms.Windows
{
    /// <summary>
    /// Helper to work with native window control's region.
    /// </summary>
    public static class WindowsRegionHelper
    {
        /// <summary>
        /// Excludes rectangles from the windows control.
        /// </summary>
        public static void Exclude(this object controlObject, params Eto.Drawing.Rectangle[] rectangles)
        {
            var windowsControl = (Control)controlObject;

            var region = new Region(windowsControl.ClientRectangle);

            foreach (var rectangle in rectangles)
            {
                region.Exclude(ToSystemRectangle(rectangle));
            }

            windowsControl.Region = region;
        }

        /// <summary>
        /// Union rectangles in the windows control.
        /// </summary>
        public static void Union(this object controlObject, params Eto.Drawing.Rectangle[] rectangles)
        {
            var windowsControl = (Control)controlObject;

            foreach (var rectangle in rectangles)
            {
                windowsControl.Region.Union(ToSystemRectangle(rectangle));
            }
        }

        private static Rectangle ToSystemRectangle(Eto.Drawing.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
