using System;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Extensions
{
    /// <summary>
    /// Uploader extensions.
    /// </summary>
    public static class UploaderExtensions
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Extended file upload.
        /// Sets last file.
        /// Disposes the uploader at the end.
        /// </summary>
        public static async Task<UploadResult> ExtendedUploadAsync(this IUploader uploader, File file, CancellationToken cancellationToken = default)
        {
            UploadingProvider.LastFile = file;

            var result = await uploader.UploadAsync(file, cancellationToken);

            if (result.ResultType == UploadResultType.Error)
            {
                logger.Error(result.ErrorMessage);
                NotifyHelper.Notify(result.ErrorMessage, "RedShot Uploading", NotifyStatus.Failed);
            }

            if (uploader is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (uploader is IDisposable disposable)
            {
                disposable.Dispose();
            }

            return result;
        }
    }
}
