using Eto.Drawing;
using Eto.Forms;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Forms;
using RedShot.Upload.Forms.Ftp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RedShot.Upload
{
    public static class UploadManager
    {
        private static UploadBar uploadBar;

        public static string LastImagePath { get; private set; }

        static UploadManager()
        {
            Uploaders = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IUploaderService).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IUploaderService)Activator.CreateInstance(t));
        }

        private static string path = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;

        public static IEnumerable<IUploaderService> Uploaders { get; }

        public static void UploadToImagesFolder(Bitmap image)
        {
            LastImagePath = Path.Combine(path, $"{DateTime.Now.ToFileTime()}.png");
            image.Save(LastImagePath, ImageFormat.Png);
        }

        public static void UploadToClipboard(Bitmap image)
        {
            Clipboard.Instance.Image = image;
        }

        public static void OpenLastImage()
        {
            if (!string.IsNullOrEmpty(LastImagePath))
            {
                Task.Run(() =>
                {
                    try
                    {
                        Process.Start(
                            new ProcessStartInfo
                            {
                                FileName = LastImagePath,
                                UseShellExecute = true
                            });
                    }
                    catch
                    {

                    }
                });
            }
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

        public static bool RunFtpUploaderView(Bitmap image)
        {
            var form = new FtpUploaderForm(image);

            form.ShowModal();

            return true;
        }

        public static IUploaderService GetUploaderService(string name)
        {
            return Uploaders.Where(u => u.ServiceName == name).FirstOrDefault();
        }

        public static Form RunUploaderView(Bitmap image)
        {
            UploadToImagesFolder(image);

            CloseUploaderView();

            uploadBar = new UploadBar(image);
            uploadBar.Show();

            return uploadBar;
        }

        public static void CloseUploaderView()
        {
            if (uploadBar != null)
            {
                try
                {
                    uploadBar.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
