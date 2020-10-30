using System;
using System.Globalization;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Screenshooting.Painting;
using RedShot.Infrastructure.Screenshooting.Views;
using RedShot.Infrastructure.Uploading;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Manages screen shotting.
    /// </summary>
    public static class ScreenshotManager
    {
        private static PaintingView paintingView;
        private static Form selectionView;

        private static readonly string imagesFolder;

        static ScreenshotManager()
        {
            imagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }

        /// <summary>
        /// Run screen shot selection view.
        /// </summary>
        public static void TakeScreenShot(bool paintAfter = true)
        {
            selectionView?.Close();

            if (paintAfter)
            {
                selectionView = new PaintingSelectionView();
            }
            else
            {
                selectionView = new ScreenShotSelectionView();
            }

            selectionView.Show();
        }

        /// <summary>
        /// Run image editor.
        /// </summary>
        internal static void RunPaintingView(Bitmap bitmap)
        {
            paintingView = new PaintingView(bitmap);
            paintingView.Show();
        }

        /// <summary>
        /// Send image to uploaders.
        /// </summary>
        internal static void UploadScreenShot(Bitmap image)
        {
            var file = GetFileFromBitmap(image);
            UploadingManager.RunUploading(file);
        }

        /// <summary>
        /// Get file from bitmap.
        /// </summary>
        internal static IFile GetFileFromBitmap(Bitmap image)
        {
            var imageName = FormatManager.GetFormattedName();
            var stringDate = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture);
            var baseName = string.Format("RedShot-Image-{0}", stringDate);
            var path = Path.Combine(imagesFolder, $"{baseName}.png");
            image.Save(path, ImageFormat.Png);

            return new ImageFile(image, path, imageName);
        }
    }
}
