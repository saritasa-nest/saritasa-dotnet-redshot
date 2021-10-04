using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Eto.Desktop.Helpers
{
    /// <summary>
    /// Represents functions to work with screen.
    /// </summary>
    public static class ScreenHelper
    {
        /// <summary>
        /// Take screen shot.
        /// </summary>
        /// <param name="screen">Display screen.</param>
        public static Bitmap TakeScreenshot(Screen screen = null)
        {
            var rect = GetScreenBounds(screen);
            // Fix black screen shot due to negative location.
            rect.Location = PointF.Empty;

            var screenshot = (screen ?? Screen.PrimaryScreen).GetImage(rect);

            return new Bitmap(screenshot);
        }

        /// <summary>
        /// Get screen bounds.
        /// </summary>
        /// <param name="screen">Display screen.</param>
        public static RectangleF GetScreenBounds(Screen screen = null)
        {
            return (screen ?? Screen.PrimaryScreen).Bounds;
        }

        /// <summary>
        /// Get center location.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="screen">Display screen.</param>
        public static Point GetCenterLocation(Size size, Screen screen = null)
        {
            if (screen == null)
            {
                screen = Screen.FromPoint(Mouse.Position);
            }

            var center = GetCentralCoordsOfScreen(screen);

            var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

            var startPoint = (Point)screen.Bounds.TopLeft;

            if (location.X >= 0 && location.Y >= 0)
            {
                return new Point(startPoint.X + location.X, startPoint.Y + location.Y);
            }
            else
            {
                return new Point(startPoint.X + center.X, startPoint.Y + center.Y);
            }
        }

        /// <summary>
        /// Get central coordinates of the screen.
        /// </summary>
        /// <param name="screen">Display screen.</param>
        public static Point GetCentralCoordsOfScreen(Screen screen = null)
        {
            var size = GetScreenBounds(screen);
            return new Point((int)(size.Width / 2), (int)(size.Height / 2));
        }
    }
}
