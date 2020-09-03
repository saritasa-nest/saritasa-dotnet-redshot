using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Rectangle painting action.
    /// Paints rectangle via first and last points.
    /// </summary>
    internal class RectanglePaintingAction : IPaintingAction
    {
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

        /// <inheritdoc />
        public PaintingActionType PaintingActionType => PaintingActionType.MousePainting;

        /// <inheritdoc />
        public void InputUserAction(IInputAction inputAction)
        {
            if (inputAction is MouseInputAction mouseAction)
            {
                endPoint = mouseAction.MouseLocation;
            }
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