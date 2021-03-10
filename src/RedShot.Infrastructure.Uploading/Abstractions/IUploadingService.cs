using Eto.Drawing;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Abstractions
{
    /// <summary>
    /// Uploading service.
    /// </summary>
    public interface IUploadingService
    {
        /// <summary>
        /// Service name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// About.
        /// </summary>
        string About { get; }

        /// <summary>
        /// Service image.
        /// </summary>
        Bitmap ServiceImage { get; }

        /// <summary>
        /// Get uploader instance.
        /// </summary>
        IUploader GetUploader();

        /// <summary>
        /// Checks on supporting file type.
        /// </summary>
        bool CheckOnSupporting(FileType fileType);
    }
}
