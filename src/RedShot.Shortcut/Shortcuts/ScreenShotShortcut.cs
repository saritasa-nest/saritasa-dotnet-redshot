using RedShot.Infrastructure.Screenshooting;

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
            ScreenshotManager.TakeScreenShot();
        }
    }
}
