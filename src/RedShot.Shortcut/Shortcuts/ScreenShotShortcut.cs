using RedShot.Infrastructure;
using RedShot.Shortcuts;

namespace RedShot.Shortcut.Shortcuts
{
    internal class ScreenShotShortcut : IShortcut
    {
        public string Name => "Screenshot";

        public void OnPressedAction()
        {
            ApplicationManager.RunScreenShooting();
        }
    }
}
