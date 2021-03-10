using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploaders.Clipboard;
using RedShot.Infrastructure.Uploaders.File;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;

namespace RedShot.Infrastructure.Uploading
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public class UploadingProvider
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get uploading services.
        /// </summary>
        public static ICollection<IUploadingService> GetUploadingServices()
        {
            return new List<IUploadingService>()
            {
                new FtpUploadingService(),
                new ClipboardUploadingService(),
                new FileUploadingService()
            };
        }

        /// <summary>
        /// Uploads file with specified uploader.
        /// </summary>
        public static async Task SafeUploadAsync(IUploader uploader, File file, CancellationToken cancellationToken = default)
        {
            try
            {
                await uploader.UploadAsync(file, cancellationToken);
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
