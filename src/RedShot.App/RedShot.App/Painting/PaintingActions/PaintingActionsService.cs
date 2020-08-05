using SkiaSharp;
using RedShot.Abstractions.Painting;
using RedShot.App.Painting.States;

namespace RedShot.App.Painting.PaintingActions
{
    internal static class PaintingActionsService
    {
        public static IPaintingAction MapFromState(PaintingState state, SKPaint paint, SKBitmap bitmap)
        {
            return state switch
            {
                PaintingState.Points => new PointPaintingAction(paint),
                PaintingState.Rectangle => new RectanglePaintingAction(paint),
                _ => null
            };
        }
    }
}
