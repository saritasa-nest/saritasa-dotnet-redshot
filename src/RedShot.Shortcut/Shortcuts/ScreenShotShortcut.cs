using Eto.Forms;
using RedShot.Infrastructure;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Screenshot shortcut.
    /// </summary>
    internal class ScreenShotShortcut : IShortcut
    {
        /// <inheritdoc/>
        public string Name => "Screenshot";

        /// <inheritdoc/>
        public Keys Keys { get; set; }

        /// <inheritdoc/>
        public void OnPressedAction()
        {
            ApplicationManager.RunScreenShooting();
        }
    }
}
