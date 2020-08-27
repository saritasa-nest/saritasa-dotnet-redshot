using System;
using Cairo;
using Gdk;
using Gtk;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.GTK
{
    /// <summary>
    /// SkiaSharp drawing control for GTK (Linux OS).
    /// </summary>
    public class SKControlGtk : EventBox
    {
        private SKBitmap bitmap;
        private SKSurface skSurface;
        private bool disposed;
        private ImageSurface surface;

        /// <summary>
        /// Executes SkiaSharp commands.
        /// </summary>
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

                    surface = new ImageSurface(bitmap.GetPixels(), Format.Argb32, bitmap.Width, bitmap.Height, bitmap.Width * 4);
                }
            }

            surfaceAction.Invoke(skSurface);
            skSurface.Canvas.Flush();

            QueueDraw();
        }

        /// <summary>
        /// Initializes SK control for GTK.
        /// </summary>
        public SKControlGtk()
        {
            AddEvents((int)EventMask.PointerMotionMask);
        }

        /// <inheritdoc />
        protected override bool OnDrawn(Context cr)
        {
            if (surface != null)
            {
                cr.SetSourceSurface(surface, 0, 0);
                cr.Paint();
            }

            return true;
        }

        /// <inheritdoc />
        public new void Dispose()
        {
            if (disposed == false)
            {
                bitmap?.Dispose();
                skSurface?.Dispose();
                surface?.Dispose();
                base.Dispose();
            }
        }
    }
}
