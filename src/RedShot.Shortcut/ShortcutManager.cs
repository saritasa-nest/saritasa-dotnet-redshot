using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RedShot.Infrastructure.Configuration;
using RedShot.Shortcut.Mapping;
using RedShot.Shortcut.Shortcuts;

namespace RedShot.Shortcut
{
    /// <summary>
    /// Shortcut manager.
    /// </summary>
    public static class ShortcutManager
    {
        /// <summary>
        /// Bind shortcuts.
        /// </summary>
        public static void BindShortcuts()
        {
            UnbindShortcuts();
            ShortcutBindingHelper.BindShortcutsList(GetMappedShortcuts());
        }

        /// <summary>
        /// Unbind shortcuts.
        /// </summary>
        public static void UnbindShortcuts()
        {
            ShortcutBindingHelper.UnbindShortcutsList(GetMappedShortcuts());
        }

        /// <summary>
        /// Get mapped shortcuts.
        /// </summary>
        internal static IEnumerable<Shortcuts.Shortcut> GetMappedShortcuts()
        {
            var shortcutsTypes = GetShortcutsTypes()
                .Select(t => (Shortcuts.Shortcut)Activator.CreateInstance(t))
                .ToList();
            var shortcutMaps = GetShortcutMaps().ToList();

            return ShortcutMappingHelper.MapShortcutsWithHotkeys(shortcutsTypes, shortcutMaps);
        }

        /// <summary>
        /// Save shortcuts maps in configuration.
        /// </summary>
        internal static void SaveShortcutMapsInConfiguration(IEnumerable<ShortcutMap> shortcutMaps)
        {
            var configOption = ConfigurationManager.GetSection<ShortcutConfiguration>();
            configOption.ShortCutMaps.Clear();
            configOption.ShortCutMaps.AddRange(shortcutMaps);

            ConfigurationManager.SetSettingsValue(configOption);
            ConfigurationManager.Save();
        }

        private static IEnumerable<Type> GetShortcutsTypes()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(Shortcuts.Shortcut).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private static IEnumerable<ShortcutMap> GetShortcutMaps()
        {
            AddMapsToConfig();
            var configOption = ConfigurationManager.GetSection<ShortcutConfiguration>();
            return configOption.ShortCutMaps;
        }

        /// <summary>
        /// Add shortcut maps to the configuration file if need.
        /// </summary>
        private static void AddMapsToConfig()
        {
            // Create a copy of shortcut maps list.
            var shortcutMaps = ConfigurationManager.GetSection<ShortcutConfiguration>().ShortCutMaps.ToList();

            foreach (var shortcut in GetShortcutsTypes())
            {
                if (shortcutMaps.Any(m => m.ShortcutType == shortcut))
                {
                    continue;
                }
                else
                {
                    var shortcutMap = new ShortcutMap()
                    {
                        ShortcutType = shortcut
                    };

                    shortcutMaps.Add(shortcutMap);
                }
            }

            SaveShortcutMapsInConfiguration(shortcutMaps);
        }
    }
}
