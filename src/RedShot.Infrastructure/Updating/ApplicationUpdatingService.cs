using System;
using System.Threading;
using System.Threading.Tasks;
using RedShot.Infrastructure.Abstractions.Updating;

namespace RedShot.Infrastructure.Updating
{
    /// <summary>
    /// Application updating service.
    /// </summary>
    public sealed class ApplicationUpdatingService : IApplicationUpdatingService, IDisposable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly TimeSpan DailyPeriod = TimeSpan.FromDays(1);

        private readonly Version currentApplicationVersion;
        private readonly IApplicationVersionRepository applicationVersionRepository;
        private readonly IApplicationUpdatingStrategy applicationUpdatingStrategy;

        private CancellationTokenSource cancellationTokenSource;
        private UpdateInterval updateInterval;
        private Timer timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationVersionRepository">Application version repository.</param>
        /// <param name="applicationUpdatingStrategy">Application updating strategy.</param>
        /// <param name="currentApplicationVersion">Current application version.</param>
        /// <param name="updateInterval">Update interval.</param>
        public ApplicationUpdatingService(
            IApplicationVersionRepository applicationVersionRepository,
            IApplicationUpdatingStrategy applicationUpdatingStrategy,
            Version currentApplicationVersion,
            UpdateInterval updateInterval)
        {
            this.currentApplicationVersion = currentApplicationVersion;
            this.updateInterval = updateInterval;
            this.applicationVersionRepository = applicationVersionRepository;
            this.applicationUpdatingStrategy = applicationUpdatingStrategy;
        }

        /// <inheritdoc/>
        public void ChangeInterval(UpdateInterval interval)
        {
            if (updateInterval == interval)
            {
                return;
            }

            updateInterval = interval;
            if (interval == UpdateInterval.Daily)
            {
                RestartUpdatingTimer();
            }
        }

        /// <inheritdoc/>
        public void StartCheckingForUpdates()
        {
            if (updateInterval == UpdateInterval.Never)
            {
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();
            timer = new Timer(ServiceTimerCallback, null, 0, 0);
        }

        private void RestartUpdatingTimer()
        {
            StopCheckingForUpdates();
            StartCheckingForUpdates();
        }

        private async void ServiceTimerCallback(object state)
        {
            try
            {
                await CheckForUpdatesAsync(cancellationTokenSource.Token);
            }
            // Do not close the application if an exception occurs.
            catch (Exception e)
            {
                logger.Error(e);
            }

            if (updateInterval == UpdateInterval.Daily)
            {
                timer.Change(DailyPeriod, TimeSpan.Zero);
            }
        }

        private async Task CheckForUpdatesAsync(CancellationToken cancellationToken)
        {
            var latestVersionData = await applicationVersionRepository.GetLatestVersionAsync(cancellationToken);

            if (currentApplicationVersion < latestVersionData.Version)
            {
                await applicationUpdatingStrategy.UpdateAsync(latestVersionData, cancellationToken);
            }
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
