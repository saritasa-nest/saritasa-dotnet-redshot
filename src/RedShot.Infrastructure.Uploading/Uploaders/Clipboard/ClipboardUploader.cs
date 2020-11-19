using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    /// <summary>
    /// Clipboard uploader.
    /// </summary>
    internal class ClipboardUploader : IUploader
    {
        /// <inheritdoc/>
        public event EventHandler<UploadingFinishedEventArgs> UploadingFinished;

        /// <inheritdoc/>
        public Task<IUploadingResponse> UploadAsync(IFile file, CancellationToken cancellationToken = default)
        {
            Eto.Forms.Clipboard.Instance.Clear();
            Eto.Forms.Clipboard.Instance.Image = file.GetFilePreview();

            UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
            return Task.FromResult(new BaseUploadingResponse(true) as IUploadingResponse);
        }
    }
}
