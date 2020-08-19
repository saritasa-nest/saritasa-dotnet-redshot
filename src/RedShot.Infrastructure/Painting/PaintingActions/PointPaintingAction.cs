using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Abstractions.Painting;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Points painting action.
    /// Paints polygon via points list.
    /// </summary>
    internal class PointPaintingAction : IPaintingAction
    {
        private HashSet<Point> points;
        private readonly SKPaint paint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        public PointPaintingAction(SKPaint paint)
        {
            points = new HashSet<Point>();
            this.paint = paint;
        }

        /// <inheritdoc cref="IPaintingAction"/>.
        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        /// <inheritdoc cref="IPaintingAction"/>.
        public void Paint(SKSurface surface)
        {
            surface.Canvas.DrawPoints(SKPointMode.Polygon, points.Select(p => new SKPoint(p.X, p.Y)).ToArray(), paint);
        }
    }
}
