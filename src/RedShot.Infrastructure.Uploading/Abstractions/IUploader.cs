using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Abstractions
{
    /// <summary>
    /// File uploader.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload result.</returns>
        Task<UploadResult> UploadAsync(File file, CancellationToken cancellationToken);
    }
}
