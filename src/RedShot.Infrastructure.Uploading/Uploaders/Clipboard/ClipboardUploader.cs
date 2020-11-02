using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;
using System;

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
        public IUploadingResponse Upload(IFile file)
        {
            Eto.Forms.Clipboard.Instance.Clear();
            Eto.Forms.Clipboard.Instance.Image = file.GetFilePreview();

            UploadingFinished?.Invoke(this, UploadingFinishedEventArgs.CreateNew(file));
            return new BaseUploadingResponse(true);
        }
    }
}
