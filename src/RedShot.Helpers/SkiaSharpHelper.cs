using Eto.Drawing;
using SkiaSharp;
using System.IO;

namespace RedShot.Helpers
{
    /// <summary>
    /// Helpers to work with Skia Sharp.
    /// </summary>
    public static class SkiaSharpHelper
    {
        /// <summary>
        /// Gives Skia bitmap from Eto bitmap.
        /// </summary>
        public static SKBitmap ConvertFromEtoBitmap(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bitmap);
                ms.Seek(0, SeekOrigin.Begin);

                return SKBitmap.Decode(ms);
            }
        }

        /// <summary>
        /// Draws pointer image for painting state.
        /// </summary>
        public static SKImage GetPointerForPainting(SKColor color, int radius)
        {
            using var surface = SKSurface.Create(radius * 2, radius * 2, SKColorType.Bgra8888, SKAlphaType.Premul);

            var paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = color
            };

            surface.Canvas.DrawCircle(radius, radius, radius, paint);

            return surface.Snapshot();
        }

        public static SKColor GetSKColorFromEtoColor(Color color)
        {
            return new SKColor((byte)color.Rb, (byte)color.Gb, (byte)color.Bb, (byte)color.Ab);
        }
    }
}
