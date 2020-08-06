using Eto.Mac.Forms;
using System;
using MonoMac.AppKit;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    public class SKControlHandler : MacView<NSView, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlMac nativecontrol;
        private bool disposed;

        public SKControlHandler()
        {
            nativecontrol = new SKControlMac();
            Control = nativecontrol;
        }

        public override Eto.Drawing.Color BackgroundColor
        {
            get => Eto.Drawing.Colors.White;
            set { }
        }

        public override NSView ContainerControl => Control;

        public override bool Enabled { get; set; }

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
