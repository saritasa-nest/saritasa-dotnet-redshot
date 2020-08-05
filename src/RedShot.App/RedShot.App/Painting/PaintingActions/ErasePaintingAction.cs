﻿using System.Collections.Generic;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Abstractions.Painting;

namespace RedShot.App.Painting.PaintingActions
{
    /// <summary>
    /// Erase painting action.
    /// Erases points.
    /// Not uses, allocate to much processor time.
    /// </summary>
    internal class ErasePaintingAction : IPaintingAction
    {
        private readonly HashSet<Point> erasingPoints;
        private readonly SKBitmap bitmap;
        private readonly SKPaint paint;

        /// <summary>
        /// Inits values for this action.
        /// </summary>
        private ErasePaintingAction(SKPaint paint, SKBitmap bitmap)
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

                var rectSize = 1 + paint.StrokeWidth * 2;
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
