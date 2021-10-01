using RedShot.Mvvm.Utils;
using System.Threading;

namespace RedShot.Mvvm.ServiceAbstractions
{
    /// <summary>
    /// Service for working with UI context.
    /// </summary>
    public interface IUiContext
    {
        /// <summary>
        /// Switch to the UI thread.
        /// </summary>
        IAwaitable SwitchToUi();

        /// <summary>
        /// Synchronization context of the UI thread.
        /// </summary>
        SynchronizationContext UiSynchronizationContext { get; }
    }
}
