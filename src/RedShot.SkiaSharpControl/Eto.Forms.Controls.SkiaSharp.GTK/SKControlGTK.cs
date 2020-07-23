using System;
using System.Windows.Markup;
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
            if (bitmap == null || skSurface == null)
            {
                var rect = Allocation;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    var ctype = SKColorType.Bgra8888;
                    var info = new SKImageInfo(rect.Width, rect.Height, ctype, SKAlphaType.Premul);
                    bitmap = new SKBitmap(info);
                    skSurface = SKSurface.Create(info, bitmap.GetPixels(), bitmap.Info.RowBytes);
                }
            }

            surfaceAction.Invoke(skSurface);

            QueueDraw();
        }

        public SKControlGTK()
        {
            AddEvents((int)EventMask.PointerMotionMask);
        }

        protected override bool OnDrawn(Context cr)
        {
            var res = base.OnDrawn(cr);
            if (res)
            {
                using (var surface = new ImageSurface(bitmap.GetPixels(), Format.Argb32, bitmap.Width, bitmap.Height, bitmap.Width * 4))
                {
                    surface.MarkDirty();
                    cr.SetSourceSurface(surface, 0, 0);
                    cr.Paint();
                }
            }
            return res;
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
