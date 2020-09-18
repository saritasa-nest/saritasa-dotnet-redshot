using System.Collections.Generic;
using System.Linq;
using RedShot.Shortcut.Shortcuts;

namespace RedShot.Shortcut.Mapping
{
    /// <summary>
    /// Shortcut mapping helper.
    /// </summary>
    internal static class ShortcutMappingHelper
    {
        /// <summary>
        /// Get shortcut maps.
        /// </summary>
        public static IEnumerable<ShortcutMap> GetShortcutMaps(IEnumerable<IShortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
                yield return new ShortcutMap(shortcut.GetType(), shortcut.Keys);
            }
        }

        /// <summary>
        /// Map shortcut objects with their hotkeys.
        /// </summary>
        public static IEnumerable<IShortcut> MapShortcutsWithHotkeys(this IEnumerable<IShortcut> shortcuts, IEnumerable<ShortcutMap> shortcutMaps)
        {
            foreach (var shortcutMap in shortcutMaps)
            {
                var shortcut = shortcuts.Single(s => s.GetType() == shortcutMap.ShortcutType);
                shortcut.Keys = shortcutMap.Keys;
            }

            return shortcuts;
        }
    }
}
