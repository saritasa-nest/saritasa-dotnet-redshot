using System;
using Eto.GtkSharp.Forms;
using Gtk;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    public class SKControlHandler : GtkControl<EventBox, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlGTK nativecontrol;
        private bool disposed;

        public SKControlHandler()
        {
            nativecontrol = new SKControlGTK();
            Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor { get; set; }

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
