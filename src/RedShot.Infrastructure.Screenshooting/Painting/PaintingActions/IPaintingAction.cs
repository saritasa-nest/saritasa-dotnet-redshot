using Eto.Drawing;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.UserInputActions;
using SkiaSharp;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions
{
    /// <summary>
    /// Abstraction for painting visual objects.
    /// </summary>
    internal interface IPaintingAction
    {
        /// <summary>
        /// Painting action type.
        /// </summary>
        PaintingActionType PaintingActionType { get; }

        /// <summary>
        /// Input user action.
        /// </summary>
        void InputUserAction(IInputAction inputAction);

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
