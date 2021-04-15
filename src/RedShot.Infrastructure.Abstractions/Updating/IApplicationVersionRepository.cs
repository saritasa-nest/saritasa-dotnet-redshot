using RedShot.Infrastructure.Abstractions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Updating
{
    /// <summary>
    /// Application version repository.
    /// </summary>
    public interface IApplicationVersionRepository
    {
        /// <summary>
        /// Get latest version.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<VersionData> GetLatestVersionAsync(CancellationToken cancellationToken);
    }
}
