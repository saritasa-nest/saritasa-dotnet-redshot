using Eto.Drawing;
using SkiaSharp;
using System.IO;

namespace RedShot.Eto.Desktop.Helpers
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
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bitmap);
            ms.Flush();
            ms.Position = 0;

            var buffer = ms.ToArray();

            return SKBitmap.Decode(buffer);
        }

        /// <summary>
        /// Draws pointer image for painting state.
        /// </summary>
        public static SKImage GetPointerForPainting(SKColor color, int radius)
        {
            var imageInfo = new SKImageInfo(radius * 2, radius * 2, SKColorType.Bgra8888, SKAlphaType.Premul);

            using var surface = SKSurface.Create(imageInfo);
            var paint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = color
            };
            surface.Canvas.DrawCircle(radius, radius, radius, paint);

            return surface.Snapshot();
        }

        /// <summary>
        /// Converts ETto drawing color to SkiaSharp color.
        /// </summary>
        public static SKColor GetSKColorFromEtoColor(Color color)
        {
            return new SKColor((byte)color.Rb, (byte)color.Gb, (byte)color.Bb, (byte)color.Ab);
        }

        /// <summary>
        /// Scale Eto image and convert it to SKImage object.
        /// </summary>
        public static SKImage GetScaledImage(Bitmap bitmap, Size size)
        {
            var imageInfo = new SKImageInfo(size.Width, size.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

            using var surface = SKSurface.Create(imageInfo);
            var image = ConvertFromEtoBitmap(bitmap).Resize(new SKImageInfo(size.Width, size.Height), SKFilterQuality.High);
            surface.Canvas.DrawBitmap(image, new SKPoint(0, 0));

            return surface.Snapshot();
        }
    }
}
