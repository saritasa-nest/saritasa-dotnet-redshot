using System;
using Eto.Mac.Forms;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    /// <summary>
    /// SkiaSharp drawing control for Mac.
    /// </summary>
    public class SKControlMac : NSView, IMacControl
    {
        private NSTrackingArea trackarea;
        private SKDrawable drawable;
        private SKSurface surface;
        private SKImageInfo skInfo;
        private bool disposed;

        /// <summary>
        /// Weak handler.
        /// </summary>
        public WeakReference WeakHandler { get; set; }

        /// <summary>
        /// Executes SkiaSharp commands.
        /// </summary>
        public void Execute(Action<SKSurface> surfaceAction)
        {
            if (surface == null)
            {
                // create the skia context
                surface = drawable.CreateSurface(Bounds, 1.0f, out SKImageInfo info);
                skInfo = info;
            }

            surfaceAction.Invoke(surface);
            this.ViewWillDraw();
        }

        /// <summary>
        /// Initializes SK control for Mac.
        /// </summary>
        public SKControlMac()
        {
            drawable = new SKDrawable();
            BecomeFirstResponder();
        }

        /// <summary>
        /// CG bounds.
        /// </summary>
        public override CGRect Bounds
        {
            get => base.Bounds;
            set
            {
                base.Bounds = value;
                UpdateTrackingAreas();
            }
        }

        /// <summary>
        /// CG frame.
        /// </summary>
        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                UpdateTrackingAreas();
            }
        }

        /// <summary>
        /// Updates tracking area.
        /// </summary>
        public override void UpdateTrackingAreas()
        {
            if (trackarea != null) { RemoveTrackingArea(trackarea); }
            trackarea = new NSTrackingArea(Frame, NSTrackingAreaOptions.ActiveWhenFirstResponder | NSTrackingAreaOptions.MouseMoved | NSTrackingAreaOptions.InVisibleRect, this, null);
            AddTrackingArea(trackarea);
        }

        /// <summary>
        /// Draws surface.
        /// </summary>
        public override void DrawRect(CGRect dirtyRect)
        {
            base.DrawRect(dirtyRect);

            var ctx = NSGraphicsContext.CurrentContext.GraphicsPort;

            if (drawable != null)
            {
                drawable.DrawSurface(ctx, Bounds, skInfo, surface);
            }
        }

        /// <inheritdoc />
        public new void Dispose()
        {
            if (disposed == false)
            {
                surface?.Dispose();
                drawable?.Dispose();
                base.Dispose();
            }
        }
    }
}
