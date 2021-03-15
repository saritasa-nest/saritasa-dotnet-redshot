using System.Linq;
using System.Collections.Generic;
using Eto.Forms;
using RedShot.Shortcut.Mapping;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Shortcut;
using RedShot.Shortcut.Units;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Settings.Sections.Shortcut
{
    /// <summary>
    /// Shortcut settings section.
    /// </summary>
    public class ShortcutSettingsSection : IValidatableSection
    {
        private readonly List<BaseShortcut> shortcuts;

        /// <inheritdoc/>
        public string Name => "Shortcuts";

        /// <summary>
        /// Create the setting section.
        /// </summary>
        public ShortcutSettingsSection()
        {
            shortcuts = ShortcutManager.Instance.GetShortcutsFromConfig().ToList();
        }

        /// <inheritdoc/>
        public Control GetControl()
        {
            var control = new ShortcutSettingsControl(shortcuts);
            control.Load += (o, e) =>
            {
                ShortcutManager.Instance.UnbindShortcuts();
            };
            control.UnLoad += (o, e) =>
            {
                ShortcutManager.Instance.BindShortcuts();
            };

            return control;
        }

        /// <inheritdoc/>
        public void Save()
        {
            var shortcutMaps = ShortcutMappingHelper.GetShortcutMaps(shortcuts);
            var configOption = UserConfiguration.Instance.GetOptionOrDefault<ShortcutConfiguration>();
            configOption.ShortcutMaps.Clear();
            configOption.ShortcutMaps.AddRange(shortcutMaps);

            UserConfiguration.Instance.SetOption(configOption);
        }

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            var distincted = shortcuts.Distinct(new ShortcutEqualityComparer());
            if (distincted.Count() != shortcuts.Count)
            {
                return new ValidationResult("Multiple shortcuts cannot contain the same keys");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
