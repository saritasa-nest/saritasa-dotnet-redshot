using System;
using Eto.WinForms.Forms;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.WinForms
{
    /// <summary>
    /// SK control handler for Windows.
    /// </summary>
    public class SKControlHandler : WindowsControl<System.Windows.Forms.Control, SKControl, Control.ICallback>, ISKControl
    {
        private readonly SKControlWinForms nativecontrol;
        private bool disposed;

        /// <summary>
        /// Inits WIN OS SkiaSharp control.
        /// </summary>
        public SKControlHandler()
        {
            nativecontrol = new SKControlWinForms();
            Control = nativecontrol;
        }

        /// <inheritdoc cref="ISKControl"/>
        public void Execute(Action<SKSurface> surface)
        {
            nativecontrol.Execute(surface);
        }

        /// <inheritdoc cref="ISKControl"/>
        void ISKControl.DisposeControl()
        {
            if (disposed == false)
            {
                nativecontrol.Dispose();
                Dispose();
                disposed = true;
            }
        }
    }
}
