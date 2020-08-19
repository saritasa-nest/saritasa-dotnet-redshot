using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using System;

namespace RedShot.Upload.Basics
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    public abstract class BaseUploader : IUploader
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
        /// Occures when uploading has finished.
        /// </summary>
        public virtual event EventHandler Uploaded;

        /// <summary>
        /// Occures when uploading has stoped.
        /// </summary>
        public virtual event EventHandler UploadStoped;

        /// <summary>
        /// Occures when uploading has started.
        /// </summary>
        public virtual event EventHandler UploadStarted;

        /// <summary>
        /// Occures by uplod stopping.
        /// </summary>
        public virtual event EventHandler UploadError;

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
