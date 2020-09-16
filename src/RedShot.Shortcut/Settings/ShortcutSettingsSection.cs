using Eto.Forms;
using RedShot.Infrastructure.Settings.Sections;
using RedShot.Shortcuts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedShot.Shortcut.Settings
{
    public class ShortcutSettingsSection : ISettingsSection
    {
        public ShortcutSettingsSection()
        {
            shortcuts = ShortcutManager.GetMappedShortcuts().ToList();
        }

        public string Name => "Shortcuts";

        private List<IShortcut> shortcuts;

        public Control GetControl()
        {
            var control = new ShortcutSettingsControl(shortcuts);
            control.Load += (o, e) =>
            {
                ShortcutManager.UnbindShortcuts(shortcuts);
            };
            control.UnLoad += (o, e) =>
            {
                ShortcutManager.BindShortcuts(shortcuts);
            };

            return new ShortcutSettingsControl(shortcuts);
        }

        public void Save()
        {
            ShortcutManager.BindShortcuts(shortcuts);
        }
    }
}
