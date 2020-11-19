using System;
using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut keys changed event arguments.
    /// </summary>
    internal class ShortcutKeysChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShortcutKeysChangedEventArgs(Keys keys)
        {
            Keys = keys;
        }

        /// <summary>
        /// Shortcut keys.
        /// </summary>
        public Keys Keys { get; }
    }
}
