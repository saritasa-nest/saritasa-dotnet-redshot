using System.Collections.Generic;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    internal class HotkeyContainer
    {
        private readonly IDictionary<Hotkey, HotkeyCallback> container;

        internal HotkeyContainer()
        {
            container = new Dictionary<Hotkey, HotkeyCallback>();
        }

        internal void Add(Hotkey hotkey, HotkeyCallback callback)
        {
            if (container.ContainsKey(hotkey))
            {
                container[hotkey] = callback;
            }
            else
            {
                container.Add(hotkey, callback);
            }
        }

        internal void Remove(Hotkey hotkey)
        {
            if (!container.ContainsKey(hotkey))
            {
                throw new HotkeyNotBoundException(
                    "This hotkey cannot be unbound because it has not previously been bound by this application");
            }

            container.Remove(hotkey);
        }

        internal HotkeyCallback Find(Hotkey hotkey)
        {
            return container[hotkey];
        }
    }
}