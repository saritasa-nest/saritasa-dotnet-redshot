using System.ComponentModel;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Application update service.
    /// </summary>
    public interface IApplicationUpdateService
    {
        /// <summary>
        /// Start check for updates.
        /// </summary>
        void StartCheckingForUpdates();

        /// <summary>
        /// Stop check for updates.
        /// </summary>
        void StopCheckForUpdates();

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
        [Description("Never")]
        Never,
        [Description("On Startup")]
        OnStartup,
        [Description("Every Day")]
        EveryDay
    }
}
