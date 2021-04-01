using System;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Application storage.
    /// </summary>
    public interface IApplicationStorage
    {
        /// <summary>
        /// Get latest version.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Version> GetLatestVersionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get release URL by version.
        /// </summary>
        /// <param name="version">Version.</param>
        string GetReleaseUrl(Version version);
    }
}
