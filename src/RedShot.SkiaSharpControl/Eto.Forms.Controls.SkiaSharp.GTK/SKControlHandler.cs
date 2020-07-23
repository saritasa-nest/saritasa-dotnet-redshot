using System;
using Eto.GtkSharp.Forms;
using Gtk;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    public class SKControlHandler : GtkControl<EventBox, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlGtk nativecontrol;
        private bool disposed;

        public SKControlHandler()
        {
            nativecontrol = new SKControlGtk();
            Control = nativecontrol;
        }

        public void Execute(Action<SKSurface> surface)
        {
            nativecontrol.Execute(surface);
        }

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
