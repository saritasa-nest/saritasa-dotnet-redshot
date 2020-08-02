using Eto.Drawing;
using SkiaSharp;
using System.IO;

namespace RedShot.Helpers
{
    public static class SkiaSharpHelper
    {
        public static SKBitmap ConvertFromEtoBitmap(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bitmap);
                ms.Seek(0, SeekOrigin.Begin);

                return SKBitmap.Decode(ms);
            }
        }

        public static SKImage GetPointerForPainting(SKColor color)
        {
            using var surface = SKSurface.Create(10, 10, SKColorType.Bgra8888, SKAlphaType.Premul);

            var paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                StrokeWidth = 2,
                Color = color
            };

            surface.Canvas.DrawCircle(5, 5, 4, paint);

            return surface.Snapshot();
        }
    }
}
