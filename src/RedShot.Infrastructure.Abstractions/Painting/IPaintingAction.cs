using Eto.Drawing;
using SkiaSharp;

namespace RedShot.Infrastructure.Abstractions.Painting
{
    /// <summary>
    /// Abstraction for painting visual objects.
    /// </summary>
    public interface IPaintingAction
    {
        /// <summary>
        /// Paints visual object.
        /// </summary>
        void Paint(SKSurface surface);

        /// <summary>
        /// Add point to the visual object.
        /// </summary>
        void AddPoint(Point point);
    }
}
