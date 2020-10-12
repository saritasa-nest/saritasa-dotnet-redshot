using System;
using System.Windows.Forms;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    internal sealed class HotkeyWindow : NativeWindow, IDisposable
    {
        const int WM_HOTKEY = 0x0312;

        internal event EventHandler<HotkeyPressedEventArgs> HotkeyPressed;

        internal HotkeyWindow()
        {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                var combination = ExtractHotkeyCombination(m);
                HotkeyPressed?.Invoke(this, new HotkeyPressedEventArgs(combination));
            }
            base.WndProc(ref m);
        }

        private static Hotkey ExtractHotkeyCombination(Message m)
        {
            var modifier = (Modifiers) ((int) m.LParam & 0xFFFF);
            var key = (Keys) ((int) m.LParam >> 16);
            return new Hotkey(modifier, key);
        }

        public void Dispose()
        {
            DestroyHandle();
        }
    }
}