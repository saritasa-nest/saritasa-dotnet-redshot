using Eto.Forms;
using RedShot.Infrastructure;
using RedShot.Shortcuts;

namespace RedShot.Shortcut.Shortcuts
{
    internal class ScreenShotShortcut : IShortcut
    {
        public string Name => "Screenshot";

        public Keys Keys { get; set; }

        public void OnPressedAction()
        {
            ApplicationManager.RunScreenShooting();
        }
    }
}
