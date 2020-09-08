using Eto.Mac.Forms;
using System;
using MonoMac.AppKit;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    /// <summary>
    /// SK control handler for Mac.
    /// </summary>
    public class SKControlHandler : MacView<NSView, SKControl, Control.ICallback>, ISKControl
    {
        private SKControlMac nativecontrol;
        private bool disposed;

        /// <summary>
        /// Initialize control and handler.
        /// </summary>
        public SKControlHandler()
        {
            nativecontrol = new SKControlMac();
            Control = nativecontrol;
        }

        /// <summary>
        /// Background color.
        /// </summary>
        public override Eto.Drawing.Color BackgroundColor
        {
            get => Eto.Drawing.Colors.White;
            set { }
        }

        /// <summary>
        /// Container control.
        /// </summary>
        public override NSView ContainerControl => Control;

        /// <summary>
        /// Enabled property.
        /// </summary>
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
