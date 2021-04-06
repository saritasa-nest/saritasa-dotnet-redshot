using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure
{
    /// <inheritdoc cref="IApplicationUpdateService"/>
    public sealed class ApplicationUpdateService : IApplicationUpdateService, IDisposable
    {
        private readonly Version currentApplicationVersion;
        private readonly IApplicationStorage applicationStorage;

        private CancellationTokenSource cancellationTokenSource;
        private UpdateInterval updateInterval;
        private Timer timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationStorage">Application storage.</param>
        /// <param name="currentApplicationVersion">Current application version.</param>
        /// <param name="updateInterval">Update interval.</param>
        public ApplicationUpdateService(
            IApplicationStorage applicationStorage,
            Version currentApplicationVersion,
            UpdateInterval updateInterval = UpdateInterval.Never)
        {
            this.currentApplicationVersion = currentApplicationVersion;
            this.updateInterval = updateInterval;
            this.applicationStorage = applicationStorage;
        }

        /// <inheritdoc/>
        public void ChangeInterval(UpdateInterval interval)
        {
            updateInterval = interval;
            timer?.Change(0, 0);
        }

        /// <inheritdoc/>
        public void StartCheckForUpdates()
        {
            cancellationTokenSource = new CancellationTokenSource();
            timer = new Timer(ServiceTimerCallback, null, 0, 0);
        }

        private async void ServiceTimerCallback(object state)
        {
            if (updateInterval == UpdateInterval.Never)
            {
                return;
            }

            await CheckForUpdatesAsync(cancellationTokenSource.Token);

            if (updateInterval == UpdateInterval.EveryDay)
            {
                timer.Change(TimeSpan.FromDays(1), TimeSpan.Zero);
            }
        }

        private async Task CheckForUpdatesAsync(CancellationToken cancellationToken)
        {
            var latestVersion = await applicationStorage.GetLatestVersionAsync(cancellationToken);

            if (currentApplicationVersion != latestVersion)
            {
                NotifyUserAboutUpdate(latestVersion);
            }
        }

        private void NotifyUserAboutUpdate(Version latestVersion)
        {
            var releaseUrl = applicationStorage.GetReleaseUrl(latestVersion);

            NotifyHelper.Notify($"New update available! New Version: {latestVersion}",
                "RedShot Update",
                onUserClick: () =>
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = releaseUrl,
                        UseShellExecute = true
                    };
                    Process.Start(processInfo);
                });
        }

        /// <inheritdoc/>
        public void StopCheckForUpdates()
        {
            cancellationTokenSource?.Cancel();
            timer?.Change(Timeout.Infinite, 0);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            timer?.Dispose();
        }
    }
}
