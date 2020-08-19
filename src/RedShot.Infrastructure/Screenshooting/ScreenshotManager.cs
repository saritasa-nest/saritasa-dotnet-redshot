using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Screenshooting
{
    internal static class ScreenshotManager
    {
        private static ScreenShotSelectionView selectionView;

        private static readonly string imagesFolder;

        static ScreenshotManager()
        {
            imagesFolder = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;
        }

        public static void TakeScreenShot()
        {
            selectionView?.Close();

            selectionView = new ScreenShotSelectionView();
            selectionView.Show();
        }

        public static void UploadScreenShot(Bitmap image)
        {
            var imageName = Guid.NewGuid().ToString();
            var path = Path.Combine(imagesFolder, imageName, "png");

            image.Save(path, ImageFormat.Png);

            ApplicationManager.RunUploadView(new ImageFile(image, path, imageName));
        }
    }
}
