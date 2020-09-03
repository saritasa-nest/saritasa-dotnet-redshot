using Eto.Drawing;

namespace RedShot.Infrastructure.Painting.PaintingActions.UserInputActions
{
    /// <summary>
    /// Mouse user input action.
    /// </summary>
    internal class MouseInputAction : IInputAction
    {
        /// <summary>
        /// Initializes keyboard user input action.
        /// </summary>
        public MouseInputAction(Point mouseLocation)
        {
            MouseLocation = mouseLocation;
        }

        /// <summary>
        /// Mouse location on the painting.
        /// </summary>
        public Point MouseLocation { get; }
    }
}