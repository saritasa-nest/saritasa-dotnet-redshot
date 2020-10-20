using System.Collections.Generic;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut equality comparer.
    /// </summary>
    internal class ShortcutEqualityComparer : IEqualityComparer<Shortcuts.Shortcut>
    {
        /// <inheritdoc/>
        public bool Equals(Shortcuts.Shortcut x, Shortcuts.Shortcut y)
        {
            return x.Keys == y.Keys;
        }

        /// <inheritdoc/>
        public int GetHashCode(Shortcuts.Shortcut obj)
        {
            return obj.Keys.GetHashCode() + 10;
        }
    }
}
