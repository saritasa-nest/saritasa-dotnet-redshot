using System;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    /// <summary>
    /// Clipboard uploader.
    /// </summary>
    internal class ClipboardUploader : IUploader
    {
        /// <inheritdoc/>
        public Task<UploadResult> UploadAsync(Uploading.Common.File file, CancellationToken cancellationToken = default)
        {
            try
            {
                Eto.Forms.Clipboard.Instance.Clear();
                Eto.Forms.Clipboard.Instance.Image = new Bitmap(file.FilePath);
            }
            catch (Exception e)
            {
                return Task.FromResult(UploadResult.Error(e.Message));
            }

            NotifyHelper.Notify("Screenshot has been copied to clipboard.", "RedShot", NotifyStatus.Success);

            return Task.FromResult(UploadResult.Successful);
        }
    }
}
