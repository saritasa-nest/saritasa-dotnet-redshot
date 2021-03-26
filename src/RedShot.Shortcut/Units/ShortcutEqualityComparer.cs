using System.Collections.Generic;
using Eto.Forms;

namespace RedShot.Shortcut.Units
{
    /// <summary>
    /// Shortcut equality comparer.
    /// </summary>
    public class ShortcutEqualityComparer : IEqualityComparer<BaseShortcut>
    {
        /// <inheritdoc/>
        public bool Equals(BaseShortcut x, BaseShortcut y)
        {
            if (x.Keys == Keys.None || y.Keys == Keys.None)
            {
                return false;
            }
            else
            {
                return x.Keys == y.Keys;
            }
        }

        /// <inheritdoc/>
        public int GetHashCode(BaseShortcut obj)
        {
            return obj.Keys.GetHashCode() + 10;
        }
    }
}
