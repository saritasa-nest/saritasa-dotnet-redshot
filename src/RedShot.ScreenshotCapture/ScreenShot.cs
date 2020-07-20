using Eto.Drawing;
using Eto.Forms;

namespace RedShot.ScreenshotCapture
{
    /// <summary>
    /// Represents functions to work with screen.
    /// </summary>
    public static class ScreenShot
    {
        /// <summary>
        /// Gives image of screen.
        /// </summary>
        /// <returns></returns>
        public static Image TakeScreenshot()
        {
            return Screen.PrimaryScreen.GetImage(GetMainWindowSize());
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
