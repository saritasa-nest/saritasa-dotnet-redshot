using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Settings.Sections;
using RedShot.Shortcut.Mapping;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut settings section.
    /// </summary>
    public class ShortcutSettingsSection : IValidatableSection
    {
        private readonly List<Shortcuts.Shortcut> shortcuts;

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

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            var distincted = shortcuts.Distinct(new ShortcutEqualityComparer());
            if (distincted.Count() != shortcuts.Count)
            {
                return new ValidationResult(false, "Multiple shortcuts cannot contain the same keys");
            }
            else
            {
                return new ValidationResult(true);
            }
        }
    }
}
