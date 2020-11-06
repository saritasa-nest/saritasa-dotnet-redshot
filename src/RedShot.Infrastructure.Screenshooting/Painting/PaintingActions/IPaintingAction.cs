using Eto.Drawing;
using SkiaSharp;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions
{
    /// <summary>
    /// Abstraction for painting visual objects.
    /// </summary>
    internal interface IPaintingAction
    {
        /// <summary>
        /// Paints visual object.
        /// </summary>
        void Paint(SKSurface surface);

        /// <summary>
        /// Add point to the visual object.
        /// </summary>
        void AddStartPoint(Point point);
    }
}
