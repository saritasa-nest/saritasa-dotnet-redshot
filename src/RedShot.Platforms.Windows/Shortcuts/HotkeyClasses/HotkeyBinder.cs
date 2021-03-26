using System;
using System.Windows.Forms;

namespace RedShot.Platforms.Windows.Shortcuts.HotkeyClasses
{
    /// <summary>
    /// Used to bind and unbind <see cref="Hotkey"/>s to
    /// <see cref="HotkeyCallback"/>s.
    /// </summary>
    internal class HotkeyBinder : IDisposable
    {
        private readonly HotkeyContainer container = new HotkeyContainer();
        private readonly HotkeyWindow hotkeyWindow = new HotkeyWindow();

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyBinder"/> class.
        /// </summary>
        public HotkeyBinder()
        {
            hotkeyWindow.HotkeyPressed += OnHotkeyPressed;
        }

        /// <summary>
        /// Indicates whether a <see cref="Hotkey"/> has been bound already either
        /// by this application or another application.
        /// </summary>
        /// <param name="hotkeyCombo">
        /// The <see cref="Hotkey"/> to evaluate.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="Hotkey"/> has not been previously bound
        /// and is available to be bound; otherwise, <c>false</c>.
        /// </returns>
        public bool IsHotkeyAlreadyBound(Hotkey hotkeyCombo)
        {
            bool successful =
                NativeMethods.RegisterHotKey(
                    hotkeyWindow.Handle,
                    hotkeyCombo.GetHashCode(),
                    (uint)hotkeyCombo.Modifier,
                    (uint)hotkeyCombo.Key);

            if (!successful)
            {
                return true;
            }

            NativeMethods.UnregisterHotKey(
                hotkeyWindow.Handle,
                hotkeyCombo.GetHashCode());

            return false;
        }

        /// <summary>
        /// Binds a hotkey combination to a <see cref="HotkeyCallback"/>.
        /// </summary>
        /// <param name="modifiers">The modifers that constitute this hotkey.</param>
        /// <param name="keys">The keys that constitute this hotkey.</param>
        public HotkeyCallback Bind(Modifiers modifiers, Keys keys)
        {
            return Bind(new Hotkey(modifiers, keys));
        }

        /// <summary>
        /// Binds a <see cref="Hotkey"/> to a <see cref="HotkeyCallback"/>.
        /// </summary>
        public HotkeyCallback Bind(Hotkey hotkeyCombo)
        {
            if (hotkeyCombo == null)
            {
                throw new ArgumentNullException("hotkeyCombo");
            }

            HotkeyCallback callback = new HotkeyCallback();
            container.Add(hotkeyCombo, callback);
            RegisterHotkey(hotkeyCombo);

            return callback;
        }

        private void RegisterHotkey(Hotkey hotkeyCombo)
        {
                NativeMethods.RegisterHotKey(
                    hotkeyWindow.Handle,
                    hotkeyCombo.GetHashCode(),
                    (uint)hotkeyCombo.Modifier,
                    (uint)hotkeyCombo.Key);
        }

        /// <summary>
        /// Unbinds a previously bound hotkey combination.
        /// </summary>
        public void Unbind(Modifiers modifiers, Keys keys)
        {
            Unbind(new Hotkey(modifiers, keys));
        }

        /// <summary>
        /// Unbinds a previously bound <see cref="Hotkey"/>.
        /// </summary>
        public void Unbind(Hotkey hotkeyCombo)
        {
            container.Remove(hotkeyCombo);
            UnregisterHotkey(hotkeyCombo);
        }

        private void UnregisterHotkey(Hotkey hotkeyCombo)
        {
            NativeMethods.UnregisterHotKey(
                hotkeyWindow.Handle,
                hotkeyCombo.GetHashCode());
        }

        private void OnHotkeyPressed(object sender, HotkeyPressedEventArgs e)
        {
            HotkeyCallback callback = container.Find(e.Hotkey);

            if (!callback.Assigned)
            {
                throw new InvalidOperationException(
                    "You did not specify a callback for the hotkey: \"" + e.Hotkey + "\". It's not your fault, " +
                    "because it wasn't possible to design the HotkeyBinder class in such a way that this is " +
                    "a statically typed pre-condition, but please specify a callback.");
            }

            callback.Invoke();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            hotkeyWindow.Dispose();
        }
    }
}