using System;
using Eto.Forms;

namespace RedShot.Shortcut.Mapping
{
    /// <summary>
    /// Shortcut map.
    /// Uses for saving shortcuts information in the configuration file.
    /// </summary>
    public class ShortcutMap
    {
        /// <summary>
        /// Uses for initialization when the object serializes from a file.
        /// </summary>
        public ShortcutMap()
        {
        }

        /// <summary>
        /// Initialize the object.
        /// </summary>
        /// <param name="shortcutType">Shortcut type <see cref="Shortcuts.Shortcut"/>.</param>
        /// <param name="keys">Hotkeys.</param>
        public ShortcutMap(Type shortcutType, Keys keys)
        {
            ShortcutType = shortcutType;
            Keys = keys;
        }

        /// <summary>
        /// Shortcut type <see cref="Shortcuts.Shortcut"/>.
        /// </summary>
        public Type ShortcutType { get; set; }

        /// <summary>
        /// Hotkeys.
        /// </summary>
        public Keys Keys { get; set; }
    }
}
