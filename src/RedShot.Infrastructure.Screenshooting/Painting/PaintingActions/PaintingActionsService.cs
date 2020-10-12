using SkiaSharp;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextInput;
using RedShot.Infrastructure.Screenshooting.Painting.States;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions
{
    /// <summary>
    /// Painting action services.
    /// </summary>
    internal static class PaintingActionsService
    {
        /// <summary>
        /// Maps PaintingState and IPaintingAction.
        /// </summary>
        public static IPaintingAction MapFromState(PaintingState state, SKPaint paint, SKBitmap bitmap)
        {
            return state switch
            {
                PaintingState.Points => new PointPaintingAction(paint),
                PaintingState.Rectangle => new RectanglePaintingAction(paint),
                PaintingState.Erase => new ErasePaintingAction(paint, bitmap),
                PaintingState.Arrow => new ArrowPaintingAction(paint),
                PaintingState.Text => new TextPaintingAction(),
                _ => null
            };
        }
    }
}
