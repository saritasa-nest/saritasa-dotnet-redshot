using System;
using System.ComponentModel;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    /// <summary>
    /// Exception thrown to indicate that specified <see cref="Hotkey"/> cannot be
    /// bound because it has been previously bound either by this application or
    /// another running application.
    /// </summary>
    [Serializable]
    public sealed class HotkeyAlreadyBoundException : Win32Exception
    {
        internal HotkeyAlreadyBoundException(int error) : base(error)
        {
        }

        internal HotkeyAlreadyBoundException(string message) : base(message)
        {
        }
    }
}