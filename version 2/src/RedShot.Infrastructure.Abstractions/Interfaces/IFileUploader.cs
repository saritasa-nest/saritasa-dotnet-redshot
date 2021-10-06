using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IFileUploader
    {
        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload result.</returns>
        Task UploadAsync(File file, CancellationToken cancellationToken = default);
    }
}
