using Eto.Drawing;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;
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
        public bool CheckOnSupporting(FileType fileType) => fileType == FileType.Image;

        /// <inheritdoc/>
        public IUploader GetUploader() => new ClipboardUploader();
    }
}
