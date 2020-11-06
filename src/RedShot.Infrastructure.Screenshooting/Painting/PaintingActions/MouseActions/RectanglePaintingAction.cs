using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.MouseActions
{
    /// <summary>
    /// Rectangle painting action.
    /// Paints rectangle via first and last points.
    /// </summary>
    internal class RectanglePaintingAction : IMousePaintingAction
    {
        private Point startPoint;
        private Point endPoint;
        private readonly SKPaint paint;

        /// <summary>
        /// Initializes rectangle painting action.
        /// </summary>
        public RectanglePaintingAction(SKPaint paint)
        {
            this.paint = paint;
        }

        /// <inheritdoc />
        public void InputMouseAction(Point mouseLocation)
        {
            endPoint = mouseLocation;
        }

        /// <inheritdoc />
        public void Paint(SKSurface surface)
        {
            var selectionRectangle = EtoDrawingHelper.CreateRectangle(startPoint, endPoint);
            var size = new SKSize(selectionRectangle.Width, selectionRectangle.Height);
            var point = new SKPoint(selectionRectangle.X, selectionRectangle.Y);
            var rectangle = SKRect.Create(point, size);
            surface.Canvas.DrawRect(rectangle, paint);
        }

        /// <inheritdoc />
        public void AddStartPoint(Point point)
        {
            startPoint = point;
            endPoint = point;
        }
    }
}