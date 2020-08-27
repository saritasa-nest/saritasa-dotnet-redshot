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
        public Bitmap ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Properties.Resources.Form);
            }
        }

        /// <inheritdoc/>
        public string About => "Uploads the file to clipboard";

        /// <inheritdoc/>
        public bool CheckOnSupporting(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Image:
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IUploader GetUploader()
        {
            return new ClipboardUploader();
        }
    }
}
