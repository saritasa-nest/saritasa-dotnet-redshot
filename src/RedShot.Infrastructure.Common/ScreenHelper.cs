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

        public static RectangleF GetScreenSize(Screen screen = null)
        {
            return screen == null ? Screen.PrimaryScreen.Bounds : screen.Bounds;
        }

        public static Point GetCenterLocation(Size size, Screen screen = null)
        {
            var center = GetCentralCoordsOfScreen(screen);

            var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

            if (location.X >= 0 && location.Y >= 0)
            {
                return location;
            }
            else
            {
                return center;
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
        /// Gives start point for upload view.
        /// </summary>
        public static Point GetStartPointForUploadView(Screen screen = null)
        {
            var size = GetScreenSize(screen);
            var minisize = GetMiniSizeDisplay(screen);
            var sixteenSize = GetSixteenthPartOfDisplay();
            return new Point((int)(size.Width - minisize.Width - sixteenSize), (int)(size.Height - minisize.Height) - 50);
        }

        /// <summary>
        /// Gives central coords of user's screen.
        /// </summary>
        public static Point GetCentralCoordsOfScreen(Screen screen = null)
        {
            var size = GetScreenSize(screen);
            return new Point((int)(size.Width / 2), (int)(size.Height / 2));
        }

        public static RectangleF GetScreenSizeByLocation(PointF mouseLocation)
        {
            var screen = Screen.FromPoint(mouseLocation);
            return screen.Bounds;
        }
    }
}
