using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions.Models;
using RedShot.Infrastructure.Abstractions.Updating;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Updating
{
    /// <summary>
    /// Notify application updating strategy.
    /// </summary>
    public class NotifyApplicationUpdatingStrategy : IApplicationUpdatingStrategy
    {
        /// <inheritdoc/>
        public Task UpdateAsync(VersionData versionData, CancellationToken cancellationToken)
        {
            NotifyHelper.Notify(
                $"New update is available!{Environment.NewLine}New version: {versionData.Version}",
                "RedShot",
                onUserClick: () => OpenReleasePage(versionData.ReleasePageUrl));

            return Task.CompletedTask;
        }

        private static void OpenReleasePage(string releasePageUrl)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = releasePageUrl,
                UseShellExecute = true
            };
            Process.Start(processInfo);
        }
    }
}
