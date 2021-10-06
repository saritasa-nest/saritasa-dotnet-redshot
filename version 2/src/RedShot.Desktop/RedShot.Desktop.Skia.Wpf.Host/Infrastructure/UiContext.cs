using RedShot.Mvvm.ServiceAbstractions;
using RedShot.Mvvm.Utils;
using System;
using System.Threading;
using System.Windows.Threading;

namespace RedShot.Desktop.WPF.Host.Infrastructure
{
    /// <summary>
    /// Working with UI context.
    /// </summary>
    internal class UiContext : IUiContext
    {
        private readonly Dispatcher dispatcher;

        /// <inheritdoc />
        public SynchronizationContext UiSynchronizationContext { get; }

        public UiContext(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            UiSynchronizationContext = new DispatcherSynchronizationContext(dispatcher);
        }

        /// <inheritdoc/>
        public IAwaitable SwitchToUi()
        {
            return new SwitchToUiAwaitable(dispatcher);
        }

        internal struct SwitchToUiAwaitable : IAwaitable
        {
            private readonly Dispatcher dispatcher;

            public SwitchToUiAwaitable(Dispatcher dispatcher)
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

            public bool IsCompleted => dispatcher.CheckAccess();

            public void OnCompleted(Action continuation)
            {
                dispatcher.BeginInvoke(continuation);
            }
        }
    }
}
