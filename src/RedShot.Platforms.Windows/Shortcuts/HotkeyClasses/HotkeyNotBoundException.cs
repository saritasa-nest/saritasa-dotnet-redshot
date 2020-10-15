using System;
using System.ComponentModel;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    /// <summary>
    /// Exception thrown to indicate that the specified <see cref="Hotkey"/> cannot
    /// be unbound because it has not previously been bound by this application.
    /// </summary>
    [Serializable]
    public sealed class HotkeyNotBoundException : Win32Exception
    {
        internal HotkeyNotBoundException(int errorCode) : base(errorCode)
        {
        }

        internal HotkeyNotBoundException(string message) : base(message)
        {
        }
    }
}