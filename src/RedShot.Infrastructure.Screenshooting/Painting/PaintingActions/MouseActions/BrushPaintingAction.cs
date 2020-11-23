using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using SkiaSharp;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.MouseActions
{
    /// <summary>
    /// Points painting action.
    /// Paints polygon via points list.
    /// </summary>
    internal class BrushPaintingAction : IMousePaintingAction
    {
        private readonly HashSet<Point> points;
        private readonly SKPaint paint;

        /// <summary>
        /// Initializes brush painting action.
        /// </summary>
        public BrushPaintingAction(SKPaint paint)
        {
            points = new HashSet<Point>();
            this.paint = paint;
        }

        /// <inheritdoc />
        public void InputMouseAction(Point mouseLocation)
        {
            points.Add(mouseLocation);
        }

        /// <inheritdoc />
        public void Paint(SKSurface surface)
        {
            surface.Canvas.DrawPoints(SKPointMode.Polygon, points.Select(p => new SKPoint(p.X, p.Y)).ToArray(), paint);
        }

        /// <inheritdoc />
        public void AddStartPoint(Point point)
        {
            points.Add(point);
        }
    }
}
