using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Forms;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public static class UploadingManager
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static UploaderChoosingForm uploaderChoosingForm;
        private static IFile lastFile;

        /// <summary>
        /// Runs uploading the latest file, if it exists.
        /// </summary>
        public static void UploadLastFile()
        {
            if (lastFile != null)
            {
                RunUploading(lastFile);
            }
        }

        /// <summary>
        /// Run uploading panel.
        /// </summary>
        public static void RunUploading(IFile file)
        {
            lastFile = file;

            uploaderChoosingForm?.Close();

            uploaderChoosingForm = new UploaderChoosingForm(file, GetUploadingServices());
            uploaderChoosingForm.Show();
        }

        /// <summary>
        /// Returns uploading services.
        /// </summary>
        public static IEnumerable<IUploadingService> GetUploadingServices()
        {
            var types = Assembly
                .GetAssembly(typeof(UploadingManager))
                ?.GetTypes()
                .Where(type => typeof(IUploadingService).IsAssignableFrom(type) && !type.IsInterface);

            foreach (var type in types)
            {
                yield return (IUploadingService)Activator.CreateInstance(type);
            }
        }

        /// <summary>
        /// Uploads file with specified uploader.
        /// </summary>
        internal static void Upload(IUploader uploader, IFile file)
        {
            if (uploader == null || file == null)
            {
                logger.Warn($"Upload failed");
                NotifyHelper.Notify($"Upload failed", "RedShot uploading", NotifyStatus.Failed);
                return;
            }

            try
            {
                var response = uploader.Upload(file);

                if (response.IsSuccess)
                {
                    logger.Trace($"{file.FileType} has been uploaded", response);

                    NotifyHelper.Notify($"{file.FileType} has been uploaded", "RedShot uploading", NotifyStatus.Success);
                }
                else
                {
                    logger.Warn($"{file.FileType} uploading was failed", response);

                    NotifyHelper.Notify($"{file.FileType} uploading failed", "RedShot uploading", NotifyStatus.Failed);
                }

                if (uploader is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                var message = $"An error was occurred while {file.FileType} was uploading. Message: {ex.Message}";

                logger.Error(ex, message);
                NotifyHelper.Notify(message, "RedShot uploading", NotifyStatus.Failed);
            }
        }
    }
}
