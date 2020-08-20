﻿using Eto.Drawing;
using RedShot.Infrastructure.Painting;
using RedShot.Infrastructure.Screenshooting;
using System;
using System.IO;

namespace RedShot.Infrastructure
{
    internal static class ScreenshotManager
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
