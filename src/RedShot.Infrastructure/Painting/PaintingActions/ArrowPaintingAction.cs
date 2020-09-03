using System;
using Eto.Drawing;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;
using SkiaSharp;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    /// <summary>
    /// Arrow painting action.
    /// Paints arrow by two points.
    /// </summary>
    internal class ArrowPaintingAction : IPaintingAction
    {
        private readonly SKPaint paint;
        private Point? lastPoint;
        private Point? startPoint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        public ArrowPaintingAction(SKPaint paint)
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
                if (startPoint.HasValue)
                {
                    lastPoint = mouseAction.MouseLocation;
                }
            }
        }

        /// <inheritdoc />
        public void Paint(SKSurface surface)
        {
            if (startPoint.HasValue && lastPoint.HasValue)
            {
                var triangleSize = paint.StrokeWidth > 3 ? paint.StrokeWidth : 4;

                var start = new SKPoint(startPoint.Value.X, startPoint.Value.Y);
                var last = new SKPoint(lastPoint.Value.X, lastPoint.Value.Y);

                // Length of the arrow.
                var length = (float) Math.Sqrt(Math.Pow(last.X - start.X, 2) + Math.Pow(last.Y - start.Y, 2));

                // Coordinates of the vector.
                var x = last.X - start.X;
                var y = last.Y - start.Y;

                // Medium point.
                var medium = new SKPoint(last.X - x / length * triangleSize * 2,
                    last.Y - y / length * triangleSize * 2);

                // Obtained multipliers x and y => coordinates of the perpendicular vector.
                var xp = last.Y - start.Y;
                var yp = start.X - last.X;

                // Coordinates of the normals, and remote from the mid point.
                var left = new SKPoint(medium.X + xp / length * triangleSize, medium.Y + yp / length * triangleSize);
                var right = new SKPoint(medium.X - xp / length * triangleSize, medium.Y - yp / length * triangleSize);

                var path = new SKPath { FillType = SKPathFillType.EvenOdd };

                path.MoveTo(start);
                path.LineTo(medium);
                path.LineTo(left);
                path.LineTo(last);
                path.LineTo(right);
                path.LineTo(medium);
                path.Close();

                surface.Canvas.DrawPath(path, paint);
            }
        }

        /// <inheritdoc />
        public void AddStartPoint(Point point)
        {
            startPoint = point;
        }
    }
}