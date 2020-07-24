using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Helpers
{
    /// <summary>
    /// Represents functions to work with screen.
    /// </summary>
    public static class ScreenHelper
    {
        /// <summary>
        /// Gives image of screen.
        /// </summary>
        /// <returns></returns>
        public static Bitmap TakeScreenshot()
        {
            return (Bitmap)Screen.PrimaryScreen.GetImage(GetMainWindowSize());
        }

        /// <summary>
        /// Gives size of screen.
        /// </summary>
        /// <returns></returns>
        public static RectangleF GetMainWindowSize()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        public static int GetSixteenthPartOfDisplay()
        {
            return (int)Screen.PrimaryScreen.Bounds.Size.Height / 16;
        }

        public static Size GetMiniSizeDisplay()
        {
            var size = GetMainWindowSize();
            return new Size((int)size.Width / 4, (int)size.Height / 4);
        }

        public static Point GetStartPointForUploadView()
        {
            var size = GetMainWindowSize();
            var minisize = GetMiniSizeDisplay();
            var sixteenSize = GetSixteenthPartOfDisplay();
            return new Point((int)(size.Width - minisize.Width - sixteenSize), (int)(size.Height - minisize.Height) - 50);
        }
    }
}
