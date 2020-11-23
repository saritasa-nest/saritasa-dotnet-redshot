using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Forms;

namespace RedShot.Infrastructure.Uploading
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public static class UploadingManager
    {
        /// <summary>
        /// Last uploaded file.
        /// </summary>
        public static IFile LastFile { get; private set; }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static UploaderChoosingForm uploaderChoosingForm;

        public static event EventHandler UploadStarted;

        /// Run uploading panel.
        /// </summary>
        public static void RunUploading(IFile file)
        {
            ProcessFile(file);
            RunManualUpload(file);
        }

        private static void ProcessFile(IFile file)
        {
            file.FileName = Formatting.FormatManager.GetFormattedName();

            LastFile = file;
            UploadStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void RunManualUpload(IFile file)
        {
            uploaderChoosingForm?.Close();
            uploaderChoosingForm = new UploaderChoosingForm(file, GetUploadingServices(GetAllUploadingTypes()));
            uploaderChoosingForm.Show();
        }

        /// <summary>
        /// Returns uploading services.
        /// </summary>
        internal static IEnumerable<IUploadingService> GetUploadingServices(IEnumerable<Type> uploadingTypes)
        {
            foreach (var type in uploadingTypes)
            {
                yield return (IUploadingService)Activator.CreateInstance(type);
            }
        }

        /// <summary>
        /// Get uploading types.
        /// </summary>
        internal static IEnumerable<Type> GetAllUploadingTypes()
        {
            var types = Assembly
                .GetAssembly(typeof(UploadingManager))
                ?.GetTypes()
                .Where(type => typeof(IUploadingService).IsAssignableFrom(type) && !type.IsInterface);

            return types;
        }

        /// <summary>
        /// Uploads file with specified uploader.
        /// </summary>
        public static async Task UploadAsync(IUploader uploader, IFile file, CancellationToken cancellationToken = default)
        {
            ProcessFile(file);

            try
            {
                var response = await uploader.UploadAsync(file, cancellationToken);

                if (response.IsSuccess)
                {
                    logger.Trace($"{file.FileType} has been uploaded", response);
                }
                else
                {
                    logger.Warn($"{file.FileType} uploading was failed", response);
                    NotifyHelper.Notify($"{file.FileType} uploading failed", "RedShot uploading", NotifyStatus.Failed);
                }
            }
            catch (Exception ex)
            {
                var message = $"An error was occurred while {file.FileType} was uploading.\nError: ";

                if (ex.InnerException != null)
                {
                    message += ex.InnerException.Message;
                }
                else
                {
                    message += ex.Message;
                }

                logger.Error(ex, message);
                NotifyHelper.Notify(message, "RedShot uploading", NotifyStatus.Failed);
            }
            finally
            {
                if (uploader is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else if (uploader is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
