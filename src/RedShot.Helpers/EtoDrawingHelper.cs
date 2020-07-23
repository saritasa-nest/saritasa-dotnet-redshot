using Eto.Drawing;

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
    }
}
