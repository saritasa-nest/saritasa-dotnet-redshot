﻿using System;
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
        /// Safely file upload.
        /// Sets last file.
        /// Disposes of the uploder instance at the end.
        /// </summary>
        public static async Task SafeUploadAsync(this IUploader uploader, File file, CancellationToken cancellationToken = default)
        {
            try
            {
                UploadingProvider.LastFile = file;
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
