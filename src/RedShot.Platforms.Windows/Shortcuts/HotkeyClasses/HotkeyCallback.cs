using System;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    /// <summary>
    /// Represents a callback for a <see cref="Hotkey"/> binding.
    /// </summary>
    internal class HotkeyCallback
    {
        private Action callback;

        public bool Assigned => callback != null;

        /// <summary>
        /// Indicates that the <see cref="Hotkey"/> should be bound to the specified
        /// <paramref name="callback"/>.
        /// </summary>
        public void To(Action callback)
        {
            this.callback = callback ?? throw new ArgumentNullException("callback");
        }

        public void Invoke()
        {
            callback.Invoke();
        }
    }
}