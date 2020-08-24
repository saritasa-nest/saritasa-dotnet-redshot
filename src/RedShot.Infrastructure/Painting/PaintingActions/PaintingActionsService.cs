using SkiaSharp;
using RedShot.Infrastructure.Abstractions.Painting;
using RedShot.Infrastructure.Painting.States;

namespace RedShot.Infrastructure.Painting.PaintingActions
{
    internal static class PaintingActionsService
    {
        public static IPaintingAction MapFromState(PaintingState state, SKPaint paint, SKBitmap bitmap)
        {
            return state switch
            {
                PaintingState.Points => new PointPaintingAction(paint),
                PaintingState.Rectangle => new RectanglePaintingAction(paint),
                PaintingState.Erase => new ErasePaintingAction(paint, bitmap),
                _ => null
            };
        }
    }
}
