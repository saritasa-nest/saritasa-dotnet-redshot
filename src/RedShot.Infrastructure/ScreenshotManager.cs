using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms.SelectionForm;
using RedShot.Infrastructure.Painting;
using RedShot.Infrastructure.Screenshooting;

namespace RedShot.Infrastructure
{
    public static class ScreenshotManager
    {
        private static PaintingView paintingView;
        private static ScreenShotSelectionView selectionView;

        private static readonly string imagesFolder;

        static ScreenshotManager()
        {
            imagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }

        public static void TakeScreenShot()
        {
            selectionView?.Close();

            ScreenShotSelectionView.SelectionScreen = Screen.PrimaryScreen;
            selectionView = new ScreenShotSelectionView();
            selectionView.Show();
        }

        public static void RunPaintingView(Bitmap bitmap)
        {
            paintingView = new PaintingView(bitmap);
            paintingView.Show();
        }

        public static void UploadScreenShot(Bitmap image)
        {
            var imageName = Guid.NewGuid().ToString();
            var path = Path.Combine(imagesFolder, $"imageName.png");

            image.Save(path, ImageFormat.Png);

            ApplicationManager.RunUploadView(new ImageFile(image, path, imageName));
        }
    }
}
