using Eto.Drawing;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.MouseActions
{
    /// <summary>
    /// Mouse painting action.
    /// </summary>
    internal interface IMousePaintingAction : IPaintingAction
    {
        /// <summary>
        /// Input mouse action.
        /// </summary>
        void InputMouseAction(Point mouseLocation);
    }
}
