using System.Collections.Generic;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Abstractions.Painting;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Erase painting action.
    /// Erases points.
    /// </summary>
    internal class ErasePaintingAction : IPaintingAction
    {
        private readonly HashSet<Point> erasingPoints;
        private readonly SKBitmap bitmap;
        private readonly SKPaint paint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        public ErasePaintingAction(SKPaint paint, SKBitmap bitmap)
        {
            erasingPoints = new HashSet<Point>();
            this.bitmap = bitmap.Copy();
            this.paint = paint;
        }

        /// <inheritdoc cref="IPaintingAction"/>.
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

        /// <inheritdoc cref="IPaintingAction"/>.
        public void AddPoint(Point point)
        {
            erasingPoints.Add(point);
        }
    }
}
