using Eto.Drawing;
using Eto.Forms;
using RedShot.Upload.Forms;
using RedShot.Upload.Forms.Ftp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RedShot.Upload
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public static class UploadManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static UploadBar uploadBar;

        public static string LastImagePath { get; private set; }

        /// <summary>
        /// Path of screenshot collection.
        /// </summary>
        private static string path = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "RedShot")).FullName;

        /// <summary>
        /// Uploads image to the default folder.
        /// </summary>
        public static void UploadToImagesFolder(Bitmap image)
        {
            LastImagePath = Path.Combine(path, $"{DateTime.Now.ToFileTime()}.png");
            image.Save(LastImagePath, ImageFormat.Png);
        }

        /// <summary>
        /// Uploads to clip board.
        /// </summary>
        public static void UploadToClipboard(Bitmap image)
        {
            Clipboard.Instance.Clear();
            Clipboard.Instance.Image = image;
        }

        /// <summary>
        /// Opens last screenshot.
        /// </summary>
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
                    catch (Exception e)
                    {
                        Logger.Error(e, "An error occured in opening image");
                    }
                });
            }
        }

        /// <summary>
        /// Upload to seleted file.
        /// </summary>
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

        /// <summary>
        /// Runs view for FTP uploading.
        /// </summary>
        public static bool RunFtpUploaderView(Bitmap image)
        {
            var form = new FtpUploaderForm(image);

            form.ShowModal();

            return true;
        }

        /// <summary>
        /// Run uploading panel.
        /// </summary>
        public static Form RunUploaderView(Bitmap image)
        {
            UploadToImagesFolder(image);

            CloseUploaderView();

            uploadBar = new UploadBar(image);
            uploadBar.Show();

            return uploadBar;
        }

        /// <summary>
        /// Closes uploader view.
        /// </summary>
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
