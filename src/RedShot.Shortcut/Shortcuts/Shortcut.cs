using Eto.Forms;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Shortcut.
    /// </summary>
    public abstract class Shortcut
    {
        /// <summary>
        /// Shortcut name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Shortcut hot keys.
        /// </summary>
        public Keys Keys { get; set; }

        /// <summary>
        /// On pressed action.
        /// </summary>
        public abstract void OnPressedAction();
    }
}
