using Eto.Forms;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Shortcut abstraction.
    /// </summary>
    internal abstract class Shortcut
    {
        /// <summary>
        /// Shortcut name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Shortcut hotkeys.
        /// </summary>
        public Keys Keys { get; set; }

        /// <summary>
        /// On pressed action.
        /// </summary>
        public abstract void OnPressedAction();
    }
}
