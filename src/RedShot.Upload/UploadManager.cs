using Eto.Drawing;
using Eto.Forms;
using RedShot.Upload.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RedShot.Upload
{
    public static class UploadManager
    {
        static UploadManager()
        {
            Uploaders = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IUploaderService).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IUploaderService)Activator.CreateInstance(t));
        }

        private static string path = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;

        public static IEnumerable<IUploaderService> Uploaders { get; }

        public static void UploadImage(Bitmap image, Rectangle rect)
        {
            image.Clone(rect).Save(Path.Combine(path, $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
        }

        public static void UploadImage(Bitmap image)
        {
            image.Save(Path.Combine(path, $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
        }

        public static void UploadToClipboard(Bitmap image)
        {
            Clipboard.Instance.Image = image;
        }

        public static bool UploadToFile(Bitmap image, Control parent)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "RedShot";
                dialog.FileName = $"{DateTime.Now.ToFileTime()}.bmp";
                dialog.Filters.Add(new FileFilter("Bmp format", ".bmp"));
                dialog.Filters.Add(new FileFilter("Png format", ".png"));

                if (dialog.ShowDialog(parent) == DialogResult.Ok)
                {
                    switch (dialog.CurrentFilterIndex)
                    {
                        case 0:
                            image.Save(dialog.FileName, ImageFormat.Bitmap);
                            break;
                        case 1:
                            image.Save(dialog.FileName, ImageFormat.Png);
                            break;
                        default:
                            image.Save(Path.Combine(dialog.Directory.ToString(), $"{DateTime.Now.ToFileTime()}.bmp"), ImageFormat.Bitmap);
                            break;
                    }
                }
            }

            return true;
        }

        public static IUploaderService GetUploaderService(string name)
        {
            return Uploaders.Where(u => u.ServiceName == name).FirstOrDefault();
        }

    }
}
