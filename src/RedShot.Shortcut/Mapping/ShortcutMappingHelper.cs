using System.Collections.Generic;
using System.Linq;
using RedShot.Shortcut.Units;

namespace RedShot.Shortcut.Mapping
{
    /// <summary>
    /// Shortcut mapping helper.
    /// </summary>
    public static class ShortcutMappingHelper
    {
        /// <summary>
        /// Get shortcut maps.
        /// </summary>
        public static IEnumerable<ShortcutMap> GetShortcutMaps(IEnumerable<BaseShortcut> shortcuts)
        {
            return shortcuts.Select(s => new ShortcutMap(s.Name, s.Keys));
        }

        /// <summary>
        /// Map shortcut objects with their hot keys.
        /// </summary>
        public static IEnumerable<BaseShortcut> MapShortcutsWithHotkeys(this IEnumerable<BaseShortcut> shortcuts, IEnumerable<ShortcutMap> shortcutMaps)
        {
            foreach (var shortcutMap in shortcutMaps)
            {
                var shortcut = shortcuts.FirstOrDefault(s => shortcutMap.ShortcutName == s.Name);

                if (shortcut != null)
                {
                    shortcut.Keys = shortcutMap.Keys;
                }
            }

            return shortcuts;
        }
    }
}
