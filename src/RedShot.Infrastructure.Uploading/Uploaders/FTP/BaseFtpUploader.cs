using System;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    public abstract class BaseFtpUploader : IUploader
    {
        /// <inheritdoc/>
        public event EventHandler<UploadingFinishedEventArgs> UploadingFinished;

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
        /// Stop upload.
        /// </summary>
        public abstract void StopUpload();

        /// <summary>
        /// Upload file to destination resource.
        /// </summary>
        public virtual IUploadingResponse Upload(IFile file)
        {
            UploadingFinished?.Invoke(this, new UploadingFinishedEventArgs() { UploadingFile = file });
            return new BaseUploadingResponse(true);
        }

        /// <summary>
        /// Connect to FTP server.
        /// </summary>
        protected abstract bool Connect();

        /// <summary>
        /// Test connection.
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                return Connect();
            }
            catch
            {
                return false;
            }
        }
    }
}
