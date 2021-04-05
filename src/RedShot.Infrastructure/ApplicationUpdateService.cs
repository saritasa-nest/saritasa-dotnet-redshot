using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Configuration.Models;

namespace RedShot.Infrastructure
{
    /// <inheritdoc cref="IApplicationUpdateService"/>
    public sealed class ApplicationUpdateService : IApplicationUpdateService, IDisposable
    {
        private readonly Version currentApplicationVersion;
        private readonly string releasesUrl;

        private CancellationTokenSource cancellationTokenSource;
        private UpdateInterval updateInterval;
        private Timer timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currentApplicationVersion"></param>
        public ApplicationUpdateService(Version currentApplicationVersion, UpdateInterval updateInterval = UpdateInterval.Never)
        {
            this.currentApplicationVersion = currentApplicationVersion;
            this.updateInterval = updateInterval;
            this.releasesUrl = AppSettings.Instance.ReleasesUrl;
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
            var lastVersionDetails = await GetLastVersionAsync(cancellationToken);

            if (currentApplicationVersion != lastVersionDetails.Version)
            {
                NotifyUserAboutUpdate(lastVersionDetails);
            }
        }

        private void NotifyUserAboutUpdate(VersionDetails versionDetails)
        {
            NotifyHelper.Notify($"New update available! New Version: {versionDetails.Version}",
                "RedShot Update",
                onUserClick: () =>
                {
                    Process.Start(versionDetails.Url);
                });
        }

        private Task<VersionDetails> GetLastVersionAsync(CancellationToken cancellationToken)
        {
            var versionDetails = new VersionDetails(new Version(1, 1, 1), "Url");
            return Task.FromResult(versionDetails);
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
