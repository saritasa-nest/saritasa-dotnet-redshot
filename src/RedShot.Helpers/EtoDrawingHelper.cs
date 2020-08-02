using Eto.Drawing;
using SkiaSharp;
using System.IO;

namespace RedShot.Helpers
{
    public static class EtoDrawingHelper
    {
        public static RectangleF CreateRectangle(PointF startLocation, PointF endLocation)
        {
            float width, height;
            float x, y;

            if (startLocation.X <= endLocation.X)
            {
                width = endLocation.X - startLocation.X + 1;
                x = startLocation.X;
            }
            else
            {
                width = startLocation.X - endLocation.X + 1;
                x = endLocation.X;
            }

            if (startLocation.Y <= endLocation.Y)
            {
                height = endLocation.Y - startLocation.Y + 1;
                y = startLocation.Y;
            }
            else
            {
                height = startLocation.Y - endLocation.Y + 1;
                y = endLocation.Y;
            }

            return new RectangleF(x, y, width, height);
        }

        public static Bitmap GetEtoBitmapFromSkiaSurface(SKSurface surface)
        {
            using (var shapshot = surface.Snapshot())
            {
                return GetEtoBitmapFromSkiaImage(shapshot);
            }
        }

        public static Bitmap GetEtoBitmapFromSkiaImage(SKImage skImage)
        {
            using (var data = skImage.Encode())
            {
                using (var stream = data.AsStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return new Eto.Drawing.Bitmap(stream);
                }
            }
        }
    }
}
