using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Painting.PaintingActions
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
