using Eto.GtkSharp;
using System;
using System.Collections.Generic;

namespace RedShot.Platforms.Linux.Shortcuts
{
    public static class LinuxShortcutBinder
    {
        private static readonly IDictionary<X11Hotkey, Action> hotkeysContainer;

        static LinuxShortcutBinder()
        {
            hotkeysContainer = new Dictionary<X11Hotkey, Action>();
        }

        public static void BindShortcut(Eto.Forms.Keys keys, Action callback)
        {
            var hotkey = GetHotkey(keys);

            UnbindHotkey(hotkey);
            hotkey.Pressed += (o, e) =>
            {
                callback.Invoke();
            };
            hotkey.Register();
        }

        public static void UnbindShortcuts(Eto.Forms.Keys keys)
        {
            var hotkey = GetHotkey(keys);

            UnbindHotkey(hotkey);
        }

        private static void UnbindHotkey(X11Hotkey hotkey)
        {
            hotkeysContainer.Remove(hotkey);
            try
            {
                hotkey.Unregister();
            }
            catch
            {
            }
        }

        private static X11Hotkey GetHotkey(Eto.Forms.Keys keys)
        {
            return new X11Hotkey(KeyMap.ToGdkKey(keys), KeyMap.ToGdkModifier(keys));
        }
    }
}
