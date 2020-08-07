using Eto.Drawing;
using Eto.Forms;
using SkiaSharp;

namespace RedShot.Helpers.Forms
{
    public static class FormsHelper
    {
        public static Control VoidBox(int size)
        {
            return new StackLayout
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Size = new Eto.Drawing.Size(size, size),
                Padding = size
            };
        }

        public static Cursor GetPointerCursor(Color color, int radius)
        {
            var skColor = SkiaSharpHelper.GetSKColorFromEtoColor(color);

            var skImage = SkiaSharpHelper.GetPointerForPainting(skColor, radius);

            var bitmap = EtoDrawingHelper.GetEtoBitmapFromSkiaImage(skImage);

            return new Cursor(bitmap, new PointF(radius, radius));
        }

        public static Cursor GetPointerCursor(SKColor color, int radius)
        {
            var skImage = SkiaSharpHelper.GetPointerForPainting(color, radius);

            var bitmap = EtoDrawingHelper.GetEtoBitmapFromSkiaImage(skImage);

            return new Cursor(bitmap, new PointF(radius, radius));
        }

        public static Point SetCenterLocation(Size size)
        {
            var center = ScreenHelper.GetCentralCoordsOfScreen();

            var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);

            if (location.X >= 0 && location.Y >= 0)
            {
                return location;
            }
            else
            {
                return center;
            }
        }
    }
}
