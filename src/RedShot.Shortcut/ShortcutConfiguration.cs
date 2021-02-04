using System.Collections.Generic;
using RedShot.Shortcut.Mapping;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut configuration.
    /// </summary>
    public class ShortcutConfiguration
    {
        /// <summary>
        /// Shortcut maps.
        /// </summary>
        public List<ShortcutMap> ShortCutMaps { get; } = new List<ShortcutMap>();
    }
}
