using System;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp
{
    /// <summary>
    /// Skia sharp control abstraction.
    /// </summary>
    public interface ISKControl : Control.IHandler
    {
        /// <summary>
        /// Executes SkiaSharp commands in control.
        /// </summary>
        void Execute(Action<SKSurface> surface);

        /// <summary>
        /// Disposes control's resources.
        /// </summary>
        void DisposeControl();
    }
}
