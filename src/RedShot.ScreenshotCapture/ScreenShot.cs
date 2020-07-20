using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RedShot.ScreenshotCapture
{
    public static class ScreenShot
    {
        public static Image TakeScreenshot()
        {
            return WindowsCapture();
        }

        public static RectangleF GetMainWindowSize()
        {
            return Screen.PrimaryScreen.Bounds;
        }


        /// <summary>
        /// Capture screenshot with .NET standard implementation.
        /// </summary>
        private static Image WindowsCapture()
        {
            return Screen.PrimaryScreen.GetImage(GetMainWindowSize());
        }
       
    }
}
