using System;
using Eto.WinForms.Forms;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.WinForms
{
    public class SKControlHandler : WindowsControl<System.Windows.Forms.Control, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlWinForms nativecontrol;
        private bool disposed;

        public SKControlHandler()
        {
            nativecontrol = new SKControlWinForms();
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
