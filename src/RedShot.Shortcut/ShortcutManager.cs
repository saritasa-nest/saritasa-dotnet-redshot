using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Shortcuts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RedShot.Shortcut
{
    public static class ShortcutManager
    {
        static ShortcutManager()
        {
            AddMapsToConfig();
        }

        internal static IEnumerable<Type> GetShortcutsTypes()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IShortcut).IsAssignableFrom(t) && !t.IsInterface);
        }

        internal static IEnumerable<IShortcut> GetMappedShortcuts()
        {
            var shortcutsTypes = GetShortcutsTypes().ToList();
            var shortcutMaps = ConfigurationManager.GetSection<ShortcutConfiguration>().ShortCutMaps;

            foreach (var map in shortcutMaps)
            {
                if (shortcutsTypes.Any(t => t == map.ShortcutType))
                {
                    yield return (IShortcut)Activator.CreateInstance(map.ShortcutType);
                }
            }
        }

        internal static void BindShortcuts(IEnumerable<IShortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
#if _WINDOWS
                Platforms.Windows.WindowsShortcutBinder.BindShortcut(shortcut.Keys, shortcut.OnPressedAction);
#endif
            }
        }

        internal static void UnbindShortcuts(IEnumerable<IShortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
#if _WINDOWS
                Platforms.Windows.WindowsShortcutBinder.UnbindShortcuts(shortcut.Keys);
#endif
            }
        }

        private static void AddMapsToConfig()
        {
            var configOption = ConfigurationManager.GetSection<ShortcutConfiguration>();
            var shortcutMaps = configOption.ShortCutMaps;

            foreach (var shortcut in GetShortcutsTypes())
            {
                if (shortcutMaps.Any(m => m.ShortcutType == shortcut))
                {
                    continue;
                }
                else
                {
                    var shortcutMap = new ShortCutMap()
                    {
                        ShortcutType = shortcut
                    };

                    shortcutMaps.Add(shortcutMap);
                }
            }

            ConfigurationManager.SetSettingsValue(configOption);
            ConfigurationManager.Save();
        }
    }
}
