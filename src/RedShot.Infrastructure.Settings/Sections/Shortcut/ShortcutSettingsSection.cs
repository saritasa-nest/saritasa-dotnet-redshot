using System.Linq;
using System.Collections.Generic;
using Eto.Forms;
using RedShot.Shortcut.Mapping;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Abstractions.Settings;
using RedShot.Shortcut;
using RedShot.Shortcut.Units;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Configuration.Models.Shortcut;

namespace RedShot.Infrastructure.Settings.Sections.Shortcut
{
    /// <summary>
    /// Shortcut settings section.
    /// </summary>
    public sealed class ShortcutSettingsSection : IValidatableSection
    {
        private readonly List<BaseShortcut> shortcuts;
        private readonly ShortcutOptionsControl shortcutOptionsControl;

        /// <inheritdoc/>
        public string Name => "Shortcuts";

        /// <summary>
        /// Create the setting section.
        /// </summary>
        public ShortcutSettingsSection()
        {
            shortcuts = ShortcutManager.Instance.GetShortcutsFromConfig().ToList();

            shortcutOptionsControl = new ShortcutOptionsControl(shortcuts);
            shortcutOptionsControl.Load += (o, e) =>
            {
                ShortcutManager.Instance.UnbindShortcuts();
            };
            shortcutOptionsControl.UnLoad += (o, e) =>
            {
                ShortcutManager.Instance.BindShortcuts();
            };
        }

        /// <inheritdoc/>
        public Control GetControl() => shortcutOptionsControl;

        /// <inheritdoc/>
        public void Save()
        {
            var shortcutMaps = ShortcutMappingHelper.GetShortcutMaps(shortcuts);
            var configuration = ConfigurationProvider.Instance.GetConfiguration<ShortcutConfiguration>();

            configuration.Shortcuts = shortcutMaps.ToList();

            ConfigurationProvider.Instance.SetConfiguration(configuration);
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

        /// <inheritdoc/>
        public void Dispose()
        {
            shortcutOptionsControl.Dispose();
        }
    }
}
