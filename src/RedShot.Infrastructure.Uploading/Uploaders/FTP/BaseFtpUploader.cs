using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common;
using System;
using System.IO;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    internal abstract class BaseFtpUploader : IUploader
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
        /// Stop upload.
        /// </summary>
        public abstract void StopUpload();

        /// <summary>
        /// Upload file to destination resource.
        /// </summary>
        public abstract IUploadingResponse Upload(IFile file);

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
