using RedShot.Infrastructure;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Screen shot shortcut.
    /// </summary>
    internal sealed class ScreenShotShortcut : Shortcut
    {
        /// <inheritdoc/>
        public override string Name => "Take screenshot";

        /// <inheritdoc/>
        public override void OnPressedAction()
        {
            ApplicationManager.RunScreenShooting();
        }
    }
}
