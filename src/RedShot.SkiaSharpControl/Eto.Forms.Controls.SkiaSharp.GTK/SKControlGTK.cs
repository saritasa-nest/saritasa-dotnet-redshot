using System;
using Cairo;
using Gdk;
using Gtk;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    public class SKControlGTK : EventBox
    {
        private SKBitmap bitmap;
        private SKSurface skSurface;
        private bool disposed;

        public void Execute(Action<SKSurface> surfaceAction)
        {
            if (bitmap == null)
            {
                throw new InvalidOperationException("Bitmap is null");

            }

            if (skSurface == null)
            {
                throw new InvalidOperationException("skSurface is null");
            }

            surfaceAction.Invoke(skSurface);

            skSurface.Canvas.Flush();
            using (var surface = new ImageSurface(bitmap.GetPixels(), Format.Argb32, bitmap.Width, bitmap.Height, bitmap.Width * 4))
            {
                surface.MarkDirty();
                var context = new Cairo.Context(surface);
                Draw(context);
            }
        }

        public SKControlGTK()
        {
            AddEvents((int)EventMask.PointerMotionMask);
        }

        protected override bool OnDrawn(Context cr)
        {
            if (bitmap == null)
            {
                var rect = Allocation;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    SKColorType ctype = SKColorType.Bgra8888;
                    bitmap = new SKBitmap(rect.Width, rect.Height, ctype, SKAlphaType.Premul);
                    skSurface = SKSurface.Create(new SKImageInfo(bitmap.Info.Width, bitmap.Info.Height, ctype, SKAlphaType.Premul), bitmap.GetPixels(out IntPtr len), bitmap.Info.RowBytes);
                }
            }
            return base.OnDrawn(cr);
        }

        public new void Dispose()
        {
            if (disposed == false)
            {
                bitmap?.Dispose();
                skSurface?.Dispose();
                base.Dispose();
            }
        }
    }
}
