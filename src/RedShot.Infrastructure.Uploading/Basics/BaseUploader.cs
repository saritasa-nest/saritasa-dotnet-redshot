using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using System;

namespace RedShot.Infrastructure.Basics
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    internal abstract class BaseUploader : IUploader
    {
        /// <summary>
        /// Uploading flag.
        /// </summary>
        public bool IsUploading { get; protected set; }

        /// <summary>
        /// Buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// Stop uploading requested flag.
        /// </summary>
        public bool StopUploadRequested { get; protected set; }

        /// <summary>
        /// Occurs when uploading has finished.
        /// </summary>
        public virtual event EventHandler Uploaded;

        /// <summary>
        /// Occurs when uploading has stopped.
        /// </summary>
        public virtual event EventHandler OnUploadStopped;

        /// <summary>
        /// Occurs when uploading has started.
        /// </summary>
        public virtual event EventHandler OnUploadStarted;

        /// <summary>
        /// Occurs by upload stopping.
        /// </summary>
        public virtual event EventHandler OnUploadError;

        /// <summary>
        /// Stops upload.
        /// </summary>
        public abstract void StopUpload();

        /// <summary>
        /// Uploads file to destination resource.
        /// </summary>
        public abstract IUploadingResponse Upload(IFile file);
    }
}
