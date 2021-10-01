using System;
using System.Threading;
using RedShot.Mvvm.ServiceAbstractions;
using RedShot.Mvvm.Utils;
using Windows.UI.Core;

namespace RedShot.Desktop.Shared.Infrastructure
{
    /// <summary>
    /// Working with UI context.
    /// </summary>
    internal class UiContext : IUiContext
    {
        private readonly CoreDispatcher dispatcher;

        public UiContext(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            UiSynchronizationContext = SynchronizationContext.Current;
        }

        /// <inheritdoc />
        public SynchronizationContext UiSynchronizationContext { get; init; }

        /// <inheritdoc/>
        public IAwaitable SwitchToUi()
        {
            return new SwitchToUiAwaitable(dispatcher);
        }

        internal struct SwitchToUiAwaitable : IAwaitable
        {
            private readonly CoreDispatcher dispatcher;

            public SwitchToUiAwaitable(CoreDispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            public IAwaitable GetAwaiter()
            {
                return this;
            }

            public void GetResult()
            {
            }

            public bool IsCompleted => dispatcher.HasThreadAccess;

            public void OnCompleted(Action continuation)
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => continuation());
            }
        }
    }
}
