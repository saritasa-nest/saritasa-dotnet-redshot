using System.Collections.Generic;

namespace RedShot.Infrastructure.Configuration.Models.Shortcut
{
    /// <summary>
    /// Shortcut configuration.
    /// </summary>
    public class ShortcutConfiguration
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShortcutConfiguration()
        {
            Shortcuts = new List<ShortcutData>();
        }

        /// <summary>
        /// Shortcuts.
        /// </summary>
        public IList<ShortcutData> Shortcuts { get; set; }
    }
}
