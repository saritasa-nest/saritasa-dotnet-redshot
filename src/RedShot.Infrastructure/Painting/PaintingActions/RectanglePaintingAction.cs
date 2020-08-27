using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Abstractions.Painting;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Rectangle painting action.
    /// Paints rectangle via first and last points.
    /// </summary>
    internal class RectanglePaintingAction : IPaintingAction
    {
        private bool selectionStarted;
        private Point startPoint;
        private Point endPoint;
        private readonly SKPaint paint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        public RectanglePaintingAction(SKPaint paint)
        {
            this.paint = paint;
        }

        /// <inheritdoc cref="IPaintingAction"/>
        public void AddPoint(Point point)
        {
            if (selectionStarted)
            {
                endPoint = point;
            }
            else
            {
                startPoint = point;
                endPoint = point;
                selectionStarted = true;
            }
        }

        /// <inheritdoc cref="IPaintingAction"/>
        public void Paint(SKSurface surface)
        {
            if (selectionStarted)
            {
                var selectionRectangle = EtoDrawingHelper.CreateRectangle(startPoint, endPoint);

                var size = new SKSize(selectionRectangle.Width, selectionRectangle.Height);
                var point = new SKPoint(selectionRectangle.X, selectionRectangle.Y);

                var rectangle = SKRect.Create(point, size);

                surface.Canvas.DrawRect(rectangle, paint);
            }
        }
    }
}