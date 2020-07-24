using Eto.Drawing;
using RedShot.Upload.Forms;
using System;
using System.IO;

namespace RedShot.Upload
{
    public static class Uploader
    {
        private static string path = Directory.CreateDirectory("Images").FullName;

        public static void UploadImage(Bitmap image, Rectangle rect)
        {
            image.Clone(rect).Save(Path.Combine(path, $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
        }

        public static void UploadImage(Bitmap image)
        {
            image.Save(Path.Combine(path, $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);           
        }
    }
}
