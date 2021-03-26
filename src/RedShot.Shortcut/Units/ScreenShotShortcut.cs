using RedShot.Infrastructure.Screenshooting;

namespace RedShot.Shortcut.Units
{
    /// <summary>
    /// Screen shot shortcut.
    /// </summary>
    public sealed class ScreenShotShortcut : BaseShortcut
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
