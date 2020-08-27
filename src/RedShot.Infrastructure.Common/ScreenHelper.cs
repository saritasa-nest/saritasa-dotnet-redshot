using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Represents functions to work with screen.
    /// </summary>
    public static class ScreenHelper
    {
        /// <summary>
        /// Gives image of user's screen.
        /// </summary>
        public static Bitmap TakeScreenshot(Screen screen = null)
        {
            if (screen == null)
            {
                return (Bitmap)Screen.PrimaryScreen.GetImage(GetScreenSize());
            }
            else
            {
                return (Bitmap)screen.GetImage(GetScreenSize(screen));
            }
        }

        /// <summary>
        /// Gives screen bounds.
        /// </summary>
        public static RectangleF GetScreenSize(Screen screen = null)
        {
            return screen == null ? Screen.PrimaryScreen.Bounds : screen.Bounds;
        }

        /// <summary>
        /// Gives location to center a view via it's size.
        /// </summary>
        public static Point GetCenterLocation(Size size, Screen screen = null)
        {
            if (screen == null)
            {
                screen = Screen.FromPoint(Mouse.Position);
            }

            var center = GetCentralCoordsOfScreen(screen);

            var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

            var startPoint = (Point)GetScreenBounds().TopLeft;

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
        /// Gives sixteenth part of user's screen size.
        /// </summary>
        public static int GetSixteenthPartOfDisplay()
        {
            return (int)(Screen.PrimaryScreen.Bounds.Size.Height / 16);
        }

        /// <summary>
        /// Gives correct small size of user's screen.
        /// </summary>
        public static Size GetMiniSizeDisplay(Screen screen = null)
        {
            var size = GetScreenSize(screen);
            return new Size((int)size.Width / 4, (int)size.Height / 4);
        }

        /// <summary>
        /// Gives central coords of user's screen.
        /// </summary>
        public static Point GetCentralCoordsOfScreen(Screen screen = null)
        {
            var size = GetScreenSize(screen);
            return new Point((int)(size.Width / 2), (int)(size.Height / 2));
        }

        /// <summary>
        /// Gives screen bounds by mouse location.
        /// </summary>
        public static RectangleF GetScreenBounds()
        {
            var screen = Screen.FromPoint(Mouse.Position);
            return screen.Bounds;
        }
    }
}
