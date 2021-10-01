using System.Runtime.CompilerServices;

namespace RedShot.Mvvm.Utils
{
    /// <summary>
    /// Base abstraction for custom awaiting.
    /// </summary>
    public interface IAwaitable : INotifyCompletion
    {
        /// <summary>
        /// Get an awaiter that can be used to await the current object / operation.
        /// </summary>
        /// <returns>Awaitable object.</returns>
        IAwaitable GetAwaiter();

        /// <summary>
        /// Indicates if the operation is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Get the execution result.
        /// </summary>
        void GetResult();
    }
}
