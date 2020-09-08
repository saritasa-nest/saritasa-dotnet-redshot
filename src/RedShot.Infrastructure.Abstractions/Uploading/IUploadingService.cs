using Eto.Drawing;

namespace RedShot.Infrastructure.Abstractions.Uploading
{
    /// <summary>
    /// Upload service abstraction.
    /// Will be changed.
    /// </summary>
    public interface IUploadingService
    {
        /// <summary>
        /// Service name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Service about.
        /// </summary>
        string About { get; }

        /// <summary>
        /// Service image.
        /// </summary>
        Bitmap ServiceImage { get; }

        /// <summary>
        /// Creates IUploader instance.
        /// </summary>
        IUploader GetUploader();

        /// <summary>
        /// Checks on supporting file type.
        /// </summary>
        bool CheckOnSupporting(FileType fileType);

        /// <summary>
        /// On uploaded action.
        /// </summary>
        void OnUploaded(IFile file);
    }
}
