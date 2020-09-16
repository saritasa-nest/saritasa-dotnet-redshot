﻿using Eto.Forms;
using Eto.WinForms;
using Shortcut;
using Shortcut.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RedShot.Platforms.Windows
{
    public static class WindowsShortcutBinder
    {
        private static HotkeyBinder hotkeyBinder = new HotkeyBinder();

        public static void BindShortcut(Eto.Forms.Keys keys, Action callback)
        {
            var hotkey = GetHotkey(keys);
            hotkeyBinder.Bind(hotkey).To(callback);
        }

        public static void UnbindShortcuts(Eto.Forms.Keys keys)
        {
            var hotkey = GetHotkey(keys);
            if (hotkeyBinder.IsHotkeyAlreadyBound(hotkey))
            {
                hotkeyBinder.Unbind(hotkey);
            }
        }

        private static Hotkey GetHotkey(Eto.Forms.Keys keys)
        {
            var swfKeys = KeyMap.ToSWF(keys);
            var converter = new HotkeyConverter();
            return (Hotkey)converter.ConvertFrom(swfKeys);
        }
    }
}
