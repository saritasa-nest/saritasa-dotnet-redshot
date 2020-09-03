using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Points painting action.
    /// Paints polygon via points list.
    /// </summary>
    internal class PointPaintingAction : IPaintingAction
    {
        private readonly HashSet<Point> points;
        private readonly SKPaint paint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        public PointPaintingAction(SKPaint paint)
        {
            points = new HashSet<Point>();
            this.paint = paint;
        }
        
        /// <inheritdoc />
        public PaintingActionType PaintingActionType => PaintingActionType.MousePainting;

        /// <inheritdoc />
        public void InputUserAction(IInputAction inputAction)
        {
            if (inputAction is MouseInputAction mouseAction)
            {
                points.Add(mouseAction.MouseLocation);
            }
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
