using System;
using Eto.GtkSharp.Forms;
using Gtk;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    /// <summary>
    /// Handles GTK control.
    /// </summary>
    public class SKControlHandler : GtkControl<EventBox, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlGtk nativecontrol;
        private bool disposed;

        /// <summary>
        /// Inits GTK SkiaSharp control.
        /// </summary>
        public SKControlHandler()
        {
            nativecontrol = new SKControlGtk();
            Control = nativecontrol;
        }

        /// <inheritdoc cref="ISKControl"/>.
        public void Execute(Action<SKSurface> surface)
        {
            nativecontrol.Execute(surface);
        }

        /// <inheritdoc cref="ISKControl"/>.
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
