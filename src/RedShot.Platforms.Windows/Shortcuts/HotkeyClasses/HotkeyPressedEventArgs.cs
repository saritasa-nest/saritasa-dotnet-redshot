using System;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    internal class HotkeyPressedEventArgs : EventArgs
    {
        internal Hotkey Hotkey { get; }

        internal HotkeyPressedEventArgs(Hotkey hotkey)
        {
            Hotkey = hotkey;
        }
    }
}