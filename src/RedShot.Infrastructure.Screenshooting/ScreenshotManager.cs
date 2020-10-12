using Eto.Drawing;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Screenshooting.Painting;
using RedShot.Infrastructure.Screenshooting.Views;
using RedShot.Infrastructure.Uploading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Screenshooting
{
    /// <summary>
    /// Manages screen shotting.
    /// </summary>
    public static class ScreenshotManager
    {
        private static PaintingView paintingView;
        private static ScreenShotSelectionView selectionView;

        private static readonly string imagesFolder;

        static ScreenshotManager()
        {
            imagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }

        /// <summary>
        /// Run screen shot selection view.
        /// </summary>
        public static void RunScreenShotting()
        {
            selectionView?.Close();

            selectionView = new ScreenShotSelectionView();
            selectionView.Show();
        }

        /// <summary>
        /// Run image editor.
        /// </summary>
        public static void RunPaintingView(Bitmap bitmap)
        {
            paintingView = new PaintingView(bitmap);
            paintingView.Show();
        }

        /// <summary>
        /// Send image to uploaders.
        /// </summary>
        public static void UploadScreenShot(Bitmap image)
        {
            var imageName = FormatManager.GetFormattedName();
            var baseName = $"RedShot-Image-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var path = Path.Combine(imagesFolder, $"{baseName}.png");

            image.Save(path, ImageFormat.Png);
            UploadingManager.RunUploading(new ImageFile(image, path, imageName));
        }
    }
}
