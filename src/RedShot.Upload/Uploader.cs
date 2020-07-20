using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Upload
{
    public static class Uploader
    {
        private static string path = Directory.CreateDirectory("Images").FullName;

        public static void UploadImage(Bitmap image, Rectangle rect)
        {
            image.Clone(rect).Save(Path.Combine(path, $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
        }
    }
}
