using System;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp
{
    public interface ISKControl : Eto.Forms.Control.IHandler
    {
        void Execute(Action<SKSurface> surface);

        void DisposeControl();
    }
}
