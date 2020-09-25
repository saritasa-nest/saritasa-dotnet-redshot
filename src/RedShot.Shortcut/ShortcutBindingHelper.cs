using System.Collections.Generic;
using Eto.Forms;
using RedShot.Shortcut.Shortcuts;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut bind helper.
    /// </summary>
    internal static class ShortcutBindingHelper
    {
        /// <summary>
        /// Bind shortcut list.
        /// </summary>
        internal static void BindShortcutsList(IEnumerable<IShortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
                if (shortcut.Keys != Keys.None)
                {
#if _WINDOWS
                    Platforms.Windows.Shortcuts.WindowsShortcutBinder.BindShortcut(shortcut.Keys, shortcut.OnPressedAction);
#elif _UNIX
                    //Platforms.Linux.Shortcuts.LinuxShortcutBinder.BindShortcut(shortcut.Keys, shortcut.OnPressedAction);
#endif
                }
            }
        }

        /// <summary>
        /// Unbind shortcut list.
        /// </summary>
        internal static void UnbindShortcutsList(IEnumerable<IShortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
                if (shortcut.Keys != Keys.None)
                {
#if _WINDOWS
                    Platforms.Windows.Shortcuts.WindowsShortcutBinder.UnbindShortcuts(shortcut.Keys);
#elif _UNIX
                    //Platforms.Linux.Shortcuts.LinuxShortcutBinder.UnbindShortcuts(shortcut.Keys);
#endif
                }
            }
        }
    }
}
