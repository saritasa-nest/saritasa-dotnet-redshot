using SkiaSharp;
using System;

namespace Eto.Forms.Controls.SkiaSharp
{
    [Handler(typeof(ISKControl))]
    public class SKControl : Control
    {
        bool disposed;

        new ISKControl Handler => (ISKControl)base.Handler;

        public void Execute(Action<SKSurface> surface)
        {
            Handler.Execute(surface);
        }

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
