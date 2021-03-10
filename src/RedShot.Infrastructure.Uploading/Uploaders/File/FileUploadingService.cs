using Eto.Drawing;
using RedShot.Resources;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploaders.File
{
    /// <summary>
    /// File uploading service.
    /// </summary>
    public class FileUploadingService : IUploadingService
    {
        /// <inheritdoc />
        public string Name => "File";

        /// <inheritdoc />
        public Bitmap ServiceImage => Icons.Folder;

        /// <inheritdoc />
        public string About => "Uploads the file to specified folder";

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return fileType switch
            {
                FileType.Image or FileType.Video => true,
                _ => false,
            };
        }

        /// <inheritdoc />
        public IUploader GetUploader() => new FileUploader();
    }
}
