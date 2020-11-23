using System;
using System.Threading;
using System.Threading.Tasks;
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
        /// Upload file to destination resource.
        /// </summary>
        public virtual Task<IUploadingResponse> UploadAsync(IFile file, CancellationToken cancellationToken)
        {
            UploadingFinished?.Invoke(this, new UploadingFinishedEventArgs() { UploadingFile = file });
            return Task.FromResult(new BaseUploadingResponse(true) as IUploadingResponse);
        }

        /// <summary>
        /// Connect to FTP server.
        /// </summary>
        protected abstract Task<bool> ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Test connection.
        /// </summary>
        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await ConnectAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
        }
    }
}
