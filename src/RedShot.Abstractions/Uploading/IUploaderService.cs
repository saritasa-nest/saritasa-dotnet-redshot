using Eto.Drawing;

namespace RedShot.Abstractions.Uploading
{
    /// <summary>
    /// Uploader service abstraction.
    /// Will be changed.
    /// </summary>
    public interface IUploaderService
    {
        /// <summary>
        /// Service name.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Service Eto Icon.
        /// </summary>
        Icon ServiceIcon { get; }

        /// <summary>
        /// Service Eto Image.
        /// </summary>
        Image ServiceImage { get; }

        /// <summary>
        /// Creates IUploader instance.
        /// </summary>
        /// <returns cref="IUploader">IUploader</returns>
        IUploader CreateUploader();
    }
}
