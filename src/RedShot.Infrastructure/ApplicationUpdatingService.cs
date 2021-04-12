using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure
{
    /// <inheritdoc cref="IApplicationUpdatingService"/>
    public sealed class ApplicationUpdatingService : IApplicationUpdatingService, IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly TimeSpan everyDayPeriod = TimeSpan.FromDays(1);

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
        public ApplicationUpdatingService(
            IApplicationStorage applicationStorage,
            Version currentApplicationVersion,
            UpdateInterval updateInterval)
        {
            this.currentApplicationVersion = currentApplicationVersion;
            this.updateInterval = updateInterval;
            this.applicationStorage = applicationStorage;
        }

        /// <inheritdoc/>
        public void ChangeInterval(UpdateInterval interval)
        {
            if (updateInterval == interval)
            {
                return;
            }

            updateInterval = interval;
            if (interval == UpdateInterval.EveryDay)
            {
                timer?.Change(0, 0);
            }
        }

        /// <inheritdoc/>
        public void StartCheckingForUpdates()
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

            try
            {
                await CheckForUpdatesAsync(cancellationTokenSource.Token);
            }
            // Do not close the application if an exception occurs.
            catch (Exception e)
            {
                logger.Error(e);
            }

            if (updateInterval == UpdateInterval.EveryDay)
            {
                timer.Change(everyDayPeriod, TimeSpan.Zero);
            }
        }

        private async Task CheckForUpdatesAsync(CancellationToken cancellationToken)
        {
            var latestVersion = await applicationStorage.GetLatestVersionAsync(cancellationToken);

            if (currentApplicationVersion < latestVersion)
            {
                NotifyUserAboutUpdate(latestVersion);
            }
        }

        private void NotifyUserAboutUpdate(Version latestVersion)
        {
            var releaseUrl = applicationStorage.GetReleaseUrl(latestVersion);

            NotifyHelper.Notify($"New update is available!{Environment.NewLine}New version: {latestVersion}",
                "RedShot",
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
        public void StopCheckingForUpdates()
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
