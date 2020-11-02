using System;

namespace RedShot.Infrastructure.Abstractions.Uploading
{
    /// <summary>
    /// Upload abstraction.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploading finished event.
        /// </summary>
        event EventHandler<UploadingFinishedEventArgs> UploadingFinished;

        /// <summary>
        /// Uploads stream to some source.
        /// </summary>
        IUploadingResponse Upload(IFile file);
    }
}
