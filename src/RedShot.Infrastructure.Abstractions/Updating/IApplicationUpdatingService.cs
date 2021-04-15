using System.ComponentModel;

namespace RedShot.Infrastructure.Abstractions.Updating
{
    /// <summary>
    /// Application updating service.
    /// </summary>
    public interface IApplicationUpdatingService
    {
        /// <summary>
        /// Start checking for updates.
        /// </summary>
        void StartCheckingForUpdates();

        /// <summary>
        /// Stop checking for updates.
        /// </summary>
        void StopCheckingForUpdates();

        /// <summary>
        /// Change update interval.
        /// </summary>
        /// <param name="interval">Update intervals.</param>
        void ChangeInterval(UpdateInterval interval);
    }

    /// <summary>
    /// Update intervals.
    /// </summary>
    public enum UpdateInterval
    {
        /// <summary>
        /// Never check for updates.
        /// </summary>
        [Description("Never")]
        Never,

        /// <summary>
        /// Check for updates on start up.
        /// </summary>
        [Description("On Startup")]
        OnStartup,

        /// <summary>
        /// Check for updates daily.
        /// </summary>
        [Description("Daily")]
        Daily
    }
}
