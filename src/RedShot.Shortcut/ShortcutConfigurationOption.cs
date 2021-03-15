using System.Collections.Generic;
using RedShot.Infrastructure.Abstractions.Configuration;
using RedShot.Shortcut.Mapping;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut configuration.
    /// </summary>
    public class ShortcutConfiguration : IConfigurationOption
    {
        /// <inheritdoc/>
        public string UniqueName => "ShortcutConfiguration";

        /// <summary>
        /// Shortcut maps.
        /// </summary>
        public List<ShortcutMap> ShortcutMaps { get; } = new List<ShortcutMap>();
    }
}
