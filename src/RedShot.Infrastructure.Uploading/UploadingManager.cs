using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Uploading.Forms;

namespace RedShot.Infrastructure.Uploading
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public static class UploadingManager
    {
        public static IFile LastFile { get; private set; }

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static UploaderChoosingForm uploaderChoosingForm;

        public static event EventHandler UploadStarted;

        /// <summary>
        /// Run uploading panel.
        /// </summary>
        public static void RunUploading(IFile file)
        {
            file.FileName = Formatting.FormatManager.GetFormattedName();

            LastFile = file;
            UploadStarted?.Invoke(null, EventArgs.Empty);

            var configuration = GetUploadingConfiguration();
            if (configuration.AutoUpload)
            {
                var uploadingServices = GetUploadingServices(configuration.UploadersTypes);
                RunAutoUpload(uploadingServices, file);
            }
            else
            {
                RunManualUpload(file);
            }
        }

        private static void RunManualUpload(IFile file)
        {
            uploaderChoosingForm?.Close();
            uploaderChoosingForm = new UploaderChoosingForm(file, GetUploadingServices(GetAllUploadingTypes()));
            uploaderChoosingForm.Show();
        }

        private static void RunAutoUpload(IEnumerable<IUploadingService> uploadingServices, IFile file)
        {
            uploadingServices = uploadingServices.Where(u => u.CheckOnSupporting(file.FileType)).ToList();

            if (uploadingServices.Count() == 0)
            {
                NotifyHelper.Notify("Auto-upload mode was enabled, but no uploader was selected", "RedShot", NotifyStatus.Failed);
                RunManualUpload(file);
                return;
            }

            foreach (var uploadingService in uploadingServices)
            {
                Upload(uploadingService, file);
            }
        }

        private static UploadingConfiguration GetUploadingConfiguration()
        {
            return ConfigurationManager.GetSection<UploadingConfiguration>();
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
        internal static void Upload(IUploadingService service, IFile file)
        {
            var uploader = service.GetUploader();

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
                    service.OnUploaded(file);
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
        }
    }
}
