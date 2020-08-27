using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms.SelectionForm;
using RedShot.Infrastructure.Painting;
using RedShot.Infrastructure.Screenshooting;

namespace RedShot.Infrastructure
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
        /// Runs screenshot selection view.
        /// </summary>
        public static void RunScreenShotting()
        {
            selectionView?.Close();

            selectionView = new ScreenShotSelectionView();
            selectionView.Show();
        }

        /// <summary>
        /// Runs image editor.
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
            var imageName = $"RedShot-image-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var path = Path.Combine(imagesFolder, $"{imageName}.png");

            image.Save(path, ImageFormat.Png);

            ApplicationManager.RunUploadView(new ImageFile(image, path, imageName));
        }
    }
}
