using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Settings.Sections;
using RedShot.Shortcut.Mapping;
using RedShot.Shortcut.Shortcuts;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut settings section.
    /// </summary>
    public class ShortcutSettingsSection : ISettingsSection
    {
        private readonly List<IShortcut> shortcuts;

        /// <inheritdoc/>
        public string Name => "Shortcuts";

        /// <summary>
        /// Create the setting section.
        /// </summary>
        public ShortcutSettingsSection()
        {
            shortcuts = ShortcutManager.GetMappedShortcuts().ToList();
        }

        /// <inheritdoc/>
        public Control GetControl()
        {
            var control = new ShortcutSettingsControl(shortcuts);
            control.Load += (o, e) =>
            {
                ShortcutManager.UnbindShortcuts();
            };
            control.UnLoad += (o, e) =>
            {
                ShortcutManager.BindShortcuts();
            };

            return control;
        }

        /// <inheritdoc/>
        public void Save()
        {
            var shortcutMaps = ShortcutMappingHelper.GetShortcutMaps(shortcuts);
            ShortcutManager.SaveShortcutMapsInConfiguration(shortcutMaps);
        }
    }
}
