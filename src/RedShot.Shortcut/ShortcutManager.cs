using System.Collections.Generic;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Configuration.Models.Shortcut;
using RedShot.Shortcut.Mapping;
using RedShot.Shortcut.Units;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut manager.
    /// </summary>
    public class ShortcutManager
    {
        /// <summary>
        /// Instance of shortcut manager.
        /// </summary>
        public static ShortcutManager Instance { get; } = new ShortcutManager();

        /// <summary>
        /// Get default shortcuts.
        /// </summary>
        private IReadOnlyCollection<BaseShortcut> Shortcuts => new List<BaseShortcut>
        {
            new RecordShortcut(),
            new ScreenShotShortcut()
        };

        /// <summary>
        /// Bind shortcuts.
        /// </summary>
        public void BindShortcuts()
        {
            UnbindShortcuts();

            var mappedShortcuts = GetShortcutsFromConfig();
            ShortcutBindingHelper.BindShortcutsList(mappedShortcuts);
        }

        /// <summary>
        /// Unbind shortcuts.
        /// </summary>
        public void UnbindShortcuts()
        {
            var mappedShortcuts = GetShortcutsFromConfig();
            ShortcutBindingHelper.UnbindShortcutsList(mappedShortcuts);
        }

        /// <summary>
        /// Get shortcuts from configuration.
        /// </summary>
        public IEnumerable<BaseShortcut> GetShortcutsFromConfig()
        {
            var configuration = ConfigurationProvider.Instance.GetConfiguration<ShortcutConfiguration>();
            var mappedShortcuts = Shortcuts.MapShortcutsWithHotkeys(configuration.Shortcuts);

            return mappedShortcuts;
        }
    }
}
