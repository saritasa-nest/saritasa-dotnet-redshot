using System.Collections.Generic;
using Eto.Drawing;
using SkiaSharp;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.MouseActions
{
    /// <summary>
    /// Erase painting action.
    /// Erases points.
    /// </summary>
    internal class ErasePaintingAction : IMousePaintingAction
    {
        private readonly HashSet<Point> erasingPoints;
        private readonly SKBitmap bitmap;
        private readonly SKPaint paint;

        /// <summary>
        /// Initializes erase painting action.
        /// </summary>
        public ErasePaintingAction(SKPaint paint, SKBitmap bitmap)
        {
            erasingPoints = new HashSet<Point>();
            this.bitmap = bitmap.Copy();
            this.paint = paint;
        }

        /// <inheritdoc />
        public void InputMouseAction(Point mouseLocation)
        {
            erasingPoints.Add(mouseLocation);
        }

        /// <inheritdoc />
        public void Paint(SKSurface surface)
        {
            foreach (var point in erasingPoints)
            {
                SKRect rect = default;
                rect.Location = new SKPoint(point.X - paint.StrokeWidth, point.Y - paint.StrokeWidth);

                var rectSize = 1 + paint.StrokeWidth;
                rect.Size = new SKSize(rectSize, rectSize);

                surface.Canvas.DrawBitmap(bitmap, rect.Standardized, rect.Standardized);
            }
        }

        /// <inheritdoc />
        public void AddStartPoint(Point point)
        {
            erasingPoints.Add(point);
        }
    }
}
