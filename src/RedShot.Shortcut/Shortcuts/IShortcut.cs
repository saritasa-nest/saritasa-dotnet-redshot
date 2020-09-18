using Eto.Forms;

namespace RedShot.Shortcut.Shortcuts
{
    /// <summary>
    /// Shortcut abstraction.
    /// </summary>
    internal interface IShortcut
    {
        /// <summary>
        /// Shortcut name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Shortcut hotkeys.
        /// </summary>
        Keys Keys { get; set; }

        /// <summary>
        /// On pressed action.
        /// </summary>
        void OnPressedAction();
    }
}
