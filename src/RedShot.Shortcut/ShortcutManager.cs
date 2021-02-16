using System.Collections.Generic;
using RedShot.Infrastructure.Configuration;
using RedShot.Shortcut.Mapping;
using RedShot.Shortcut.Shortcuts;

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
        private IReadOnlyCollection<Shortcuts.Shortcut> Shortcuts => new List<Shortcuts.Shortcut>
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
        public IEnumerable<Shortcuts.Shortcut> GetShortcutsFromConfig()
        {
            var configOption = UserConfiguration.Instance.GetOptionOrDefault<ShortcutConfiguration>();
            var mappedShortcuts = Shortcuts.MapShortcutsWithHotkeys(configOption.ShortcutMaps);

            return mappedShortcuts;
        }
    }
}
