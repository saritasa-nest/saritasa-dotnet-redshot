using System.Drawing;
using System.Windows.Forms;

namespace RedShot.Recording.Windows
{
    public static class RegionHelper
    {
        public static void Exclude(this object windowsControl, params Eto.Drawing.Rectangle[] rectangles)
        {
            if (windowsControl is Control control)
            {
                var region = new Region(new Rectangle(control.Location, control.Size));

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
