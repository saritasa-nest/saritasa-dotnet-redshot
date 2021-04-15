using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions.Models;

namespace RedShot.Infrastructure.Abstractions.Updating
{
    /// <summary>
    /// Application updating strategy.
    /// </summary>
    public interface IApplicationUpdatingStrategy
    {
        /// <summary>
        /// Update application.
        /// </summary>
        /// <param name="versionData">Version data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task UpdateAsync(VersionData versionData, CancellationToken cancellationToken);
    }
}
