using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
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

        public static IEnumerable<IUploadingService> GetUploadingServices()
        {
            var types = Assembly
                .GetAssembly(typeof(UploadManager))
                ?.GetTypes()
                .Where(type => typeof(IUploadingService).IsAssignableFrom(type) && !type.IsInterface);

            foreach (var type in types)
            {
                yield return (IUploadingService)Activator.CreateInstance(type);
            }
        }

        internal static void Upload(IUploader uploader, IFile file)
        {
            if (uploader == null || file == null)
            {
                return;
            }

            try
            {
                var response = uploader.Upload(file);

                if (response.IsSuccess)
                {
                    Logger.Trace($"{file.FileType} has been uploaded", response);
                    MessageBox.Show($"{file.FileType} has been uploaded", "Success", MessageBoxButtons.OK, MessageBoxType.Information);
                }
                else
                {
                    Logger.Warn($"{file.FileType} uploading was failed", response);
                    MessageBox.Show($"{file.FileType} uploading failed", MessageBoxButtons.OK, MessageBoxType.Information);
                }

                if (uploader is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                var message = $"An error was occurred while {file.FileType} was uploading. File path: {file.FilePath}";

                Logger.Error(ex, message);
                MessageBox.Show(ex.Message, message, MessageBoxButtons.OK, MessageBoxType.Error);
            }
        }
    }
}
