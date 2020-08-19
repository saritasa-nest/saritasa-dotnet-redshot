using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Forms;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public static class UploadManager
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static UploadBar uploadBar;

        public static void OpenFile(IFile file)
        {
            if (!string.IsNullOrEmpty(file.FilePath))
            {
                Task.Run(() =>
                {
                    try
                    {
                        Process.Start(
                            new ProcessStartInfo
                            {
                                FileName = file.FilePath,
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
        /// Run uploading panel.
        /// </summary>
        public static Form RunUploading(IFile file)
        {
            uploadBar?.Close();

            uploadBar = new UploadBar(file);
            uploadBar.Show();

            return uploadBar;
        }
    }
}
