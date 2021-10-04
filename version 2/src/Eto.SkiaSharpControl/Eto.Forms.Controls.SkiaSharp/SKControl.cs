using System;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp
{
    /// <summary>
    /// Common implementation for SkiaSharp control.
    /// </summary>
    [Handler(typeof(ISKControl))]
    public class SKControl : Control
    {
        private new ISKControl Handler => (ISKControl)base.Handler;

        private bool disposed;

        /// <inheritdoc cref="ISKControl"/>
        public void Execute(Action<SKSurface> surface)
        {
            Handler.Execute(surface);
        }

        /// <inheritdoc />
        public new void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;

                Handler?.DisposeControl();
            }
        }
    }
}
