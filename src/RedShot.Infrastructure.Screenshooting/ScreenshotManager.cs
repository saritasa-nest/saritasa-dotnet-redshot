using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Screenshooting.Painting;
using RedShot.Infrastructure.Screenshooting.Views;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Manages screen shotting.
    /// </summary>
    public static class ScreenshotManager
    {
        private static PaintingView paintingView;
        private static Form screenshotView;

        /// <summary>
        /// Run screen shot selection view.
        /// </summary>
        public static void TakeScreenShot()
        {
            screenshotView?.Close();

            screenshotView = new ScreenShotSelectionView();
            screenshotView.Show();
        }

        /// <summary>
        /// Run image editor.
        /// </summary>
        public static void RunPaintingView(Bitmap bitmap)
        {
            paintingView = new PaintingView(bitmap);
            paintingView.Show();
        }
    }
}
