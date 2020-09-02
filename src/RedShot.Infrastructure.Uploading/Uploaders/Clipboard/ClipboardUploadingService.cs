using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    /// <summary>
    /// Clipboard uploading service.
    /// </summary>
    internal class ClipboardUploadingService : IUploadingService
    {
        /// <inheritdoc/>
        public string Name => "Clipboard";

        /// <inheritdoc/>
        public Bitmap ServiceImage => new Bitmap(Resources.Properties.Resources.Form);

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
            return new ClipboardUploader();
        }
    }
}
