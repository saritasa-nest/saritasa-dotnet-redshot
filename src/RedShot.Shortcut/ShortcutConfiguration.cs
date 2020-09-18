using System.Collections.Generic;
using RedShot.Infrastructure.Abstractions;
using RedShot.Shortcut.Mapping;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut configuration.
    /// </summary>
    public class ShortcutConfiguration : IConfigurationOption
    {
        /// <inheritdoc/>
        public string UniqueName => "Shortcut configuration";

        /// <summary>
        /// Shortcut maps.
        /// </summary>
        public List<ShortcutMap> ShortCutMaps { get; } = new List<ShortcutMap>();

        /// <inheritdoc/>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
