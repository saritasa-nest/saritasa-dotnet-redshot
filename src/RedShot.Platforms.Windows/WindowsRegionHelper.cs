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
        public static void Exclude(this object windowsControl, params Eto.Drawing.Rectangle[] rectangles)
        {
            if (windowsControl is Control control)
            {
                var region = new Region(control.ClientRectangle);

                foreach (var rectangle in rectangles)
                {
                    region.Exclude(ToSystemRectangle(rectangle));
                }

                control.Region = region;
            }
        }

        private static Rectangle ToSystemRectangle(Eto.Drawing.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
