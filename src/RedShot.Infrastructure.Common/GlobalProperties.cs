using System;
using System.Threading;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Global properties.
    /// </summary>
    public class GlobalProperties
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Application cancellation token.
        /// </summary>
        public CancellationToken ApplicationCancellationToken => cancellationTokenSource.Token;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobalProperties()
        {
            cancellationTokenSource = new CancellationTokenSource();
            AppDomain.CurrentDomain.ProcessExit += (o, e) =>
            {
                cancellationTokenSource.Cancel();
            };
        }
    }
}
