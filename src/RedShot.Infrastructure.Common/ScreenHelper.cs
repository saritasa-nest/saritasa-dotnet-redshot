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

        ///// <summary>
        ///// Gives rectangle of user's screen.
        ///// </summary>
        //public static RectangleF GetMainWindowSize()
        //{
        //    return Screen.PrimaryScreen.Bounds;
        //}

        public static RectangleF GetScreenSize(Screen screen = null)
        {
            return screen == null ? Screen.PrimaryScreen.Bounds : screen.Bounds;
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
        public static Size GetMiniSizeDisplay()
        {
            var size = GetScreenSize();
            return new Size((int)size.Width / 4, (int)size.Height / 4);
        }

        /// <summary>
        /// Gives start point for upload view.
        /// </summary>
        public static Point GetStartPointForUploadView()
        {
            var size = GetScreenSize();
            var minisize = GetMiniSizeDisplay();
            var sixteenSize = GetSixteenthPartOfDisplay();
            return new Point((int)(size.Width - minisize.Width - sixteenSize), (int)(size.Height - minisize.Height) - 50);
        }

        /// <summary>
        /// Gives central coords of user's screen.
        /// </summary>
        public static Point GetCentralCoordsOfScreen()
        {
            var size = GetScreenSize();
            return new Point((int)(size.Width / 2), (int)(size.Height / 2));
        }

        public static RectangleF GetScreenSizeByLocation(PointF mouseLocation)
        {
            var screen = Screen.FromPoint(mouseLocation);
            return screen.Bounds;
        }
    }
}
