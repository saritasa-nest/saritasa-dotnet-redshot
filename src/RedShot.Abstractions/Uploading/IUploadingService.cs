using Eto.Drawing;

namespace RedShot.Abstractions.Uploading
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
        string ServiceName { get; }

        /// <summary>
        /// Service Eto Image.
        /// </summary>
        Image ServiceImage { get; }

        /// <summary>
        /// Creates IUploader instance.
        /// </summary>
        IUploader GetUploader();

        bool CheckOnSupporting(FileType fileType);
    }
}
