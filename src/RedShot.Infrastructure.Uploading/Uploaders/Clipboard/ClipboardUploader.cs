using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Abstractions;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    /// <summary>
    /// Clipboard uploader.
    /// </summary>
    internal class ClipboardUploader : IUploader
    {
        /// <inheritdoc/>
        public Task UploadAsync(Uploading.Common.File file, CancellationToken cancellationToken = default)
        {
            Eto.Forms.Clipboard.Instance.Clear();
            Eto.Forms.Clipboard.Instance.Image = new Bitmap(file.FilePath);

            NotifyHelper.Notify("Screenshot has been copied to clipboard.", "RedShot", NotifyStatus.Success);

            return Task.CompletedTask;
        }
    }
}
