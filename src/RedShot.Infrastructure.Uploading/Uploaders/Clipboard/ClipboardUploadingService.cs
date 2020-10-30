using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Resources;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    /// <summary>
    /// Clipboard uploading service.
    /// </summary>
    public class ClipboardUploadingService : IUploadingService
    {
        /// <inheritdoc/>
        public string Name => "Clipboard";

        /// <inheritdoc/>
        public Bitmap ServiceImage => Icons.Form;

        /// <inheritdoc/>
        public string About => "Uploads the file to clipboard";

        /// <inheritdoc/>
        public bool CheckOnSupporting(FileType fileType)
        {
            return fileType switch
            {
                FileType.Image => true,
                _ => false
            };
        }

        /// <inheritdoc/>
        public IUploader GetUploader()
        {
            var uploader = new ClipboardUploader();
            uploader.UploadingFinished += ClipboardUploaderUploadingFinished;

            return uploader;
        }

        private void ClipboardUploaderUploadingFinished(object sender, UploadingFinishedEventArgs e)
        {
            NotifyHelper.Notify("The file has been saved in clipboard.", "RedShot", NotifyStatus.Success);
        }
    }
}
