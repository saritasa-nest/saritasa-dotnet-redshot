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
    }
}
