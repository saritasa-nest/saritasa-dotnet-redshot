using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedShot.ScreenshotCapture
{
    public static class ScreenShot
    {
        public static Image TakeScreenshot(bool onlyPrimaryScreen = false)
        {
            return WindowsCapture(onlyPrimaryScreen);
        }

        public static Rectangle GetMainWindowSize()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        #region  Private static methods
        

        /// <summary>
        /// Capture screenshot with .NET standard implementation.
        /// </summary>
        private static Image WindowsCapture(bool onlyPrimaryScreen)
        {
            if (onlyPrimaryScreen) return ScreenCapture(Screen.PrimaryScreen);
            var bitmaps = (Screen.AllScreens.OrderBy(s => s.Bounds.Left).Select(ScreenCapture)).ToArray();
            return CombineBitmap(bitmaps);
        }

        /// <summary>
        /// Create screenshot of single display.
        /// </summary>
        private static Bitmap ScreenCapture(Screen screen)
        {
            var bounds = screen.Bounds;

            if (screen.Bounds.Width / screen.WorkingArea.Width > 1 || screen.Bounds.Height / screen.WorkingArea.Height > 1)
            {
                // Trick  to restore original bounds of screen.
                bounds = new Rectangle(
                    0,
                    0,
                    screen.WorkingArea.Width + screen.WorkingArea.X,
                    screen.WorkingArea.Height + screen.WorkingArea.Y);
            }

            var pixelFormat = new Bitmap(1, 1, Graphics.FromHwnd(IntPtr.Zero)).PixelFormat;

            var bitmap = new Bitmap(bounds.Width, bounds.Height, pixelFormat);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(
                    bounds.X,
                    bounds.Y,
                    0,
                    0,
                    bounds.Size,
                    CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        /// <summary>
        /// Combime images collection in one bitmap.
        /// </summary>
        /// <param name="images"></param>
        /// <returns>Combined image</returns>
        private static Image CombineBitmap(ICollection<Image> images)
        {
            if (images.Count == 1)
                return images.First();

            Image finalImage = null;

            try
            {
                var width = 0;
                var height = 0;

                foreach (var image in images)
                {
                    width += image.Width;
                    height = image.Height > height ? image.Height : height;
                }

                finalImage = new Bitmap(width, height);

                using (var g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.Black);

                    var offset = 0;
                    foreach (var image in images)
                    {
                        g.DrawImage(image,
                            new Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();
                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (var image in images)
                {
                    image.Dispose();
                }
            }

            return finalImage;
        }

        #endregion
    }
}
