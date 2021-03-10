using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Abstractions
{
    /// <summary>
    /// Upload abstraction.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploads stream to some source.
        /// </summary>
        Task UploadAsync(File file, CancellationToken cancellationToken);
    }
}
