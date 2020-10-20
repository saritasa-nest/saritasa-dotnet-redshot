using RedShot.Infrastructure;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Paint shortcut.
    /// </summary>
    internal sealed class PaintShortcut : Shortcut
    {
        /// <inheritdoc/>
        public override string Name => "Paint captured area";

        /// <inheritdoc/>
        public override void OnPressedAction()
        {
            ApplicationManager.RunPainting();
        }
    }
}
