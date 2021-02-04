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
        /// Constructor.
        /// </summary>
        public ShortcutMap()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shortcutName">Shortcut name.</param>
        /// <param name="keys">Hotkeys.</param>
        public ShortcutMap(string shortcutName, Keys keys)
        {
            ShortcutName = shortcutName;
            Keys = keys;
        }

        /// <summary>
        /// Shortcut name.
        /// </summary>
        public string ShortcutName { get; set; }

        /// <summary>
        /// Hotkeys.
        /// </summary>
        public Keys Keys { get; set; }
    }
}
