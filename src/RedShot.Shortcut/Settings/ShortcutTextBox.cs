using System;
using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut text box.
    /// </summary>
    internal class ShortcutTextBox : TextBox
    {
        private Keys keys;
        private readonly ShortcutKeysHelper keysHelper;

        /// <summary>
        /// Keys changing event.
        /// </summary>
        public event EventHandler KeysChanging;

        /// <summary>
        /// Hot keys.
        /// </summary>
        public Keys Keys
        {
            get { return keys; }

            set
            {
                if (keys != value)
                {
                    keys = value;
                    RenderText();
                    KeysChanging?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShortcutTextBox()
        {
            keysHelper = new ShortcutKeysHelper();
            Reset();
#if _WINDOWS
            RedShot.Platforms.Windows.WindowsNativeHelper.HideCaret(this.ControlObject);
#endif
        }

        /// <summary>
        /// Reset the value of the text box.
        /// </summary>
        public void Reset()
        {
            Keys = Keys.None;
            Text = "None";
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = true;

            if (e.Key == Keys.Backspace)
            {
                Reset();
                return;
            }

            Keys = e.KeyData;
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;

            if (e.KeyData.HasFlag(Keys.PrintScreen) && ValidateShortcutKeys(e.KeyData))
            {
                Keys = e.KeyData;
            }
            else if (!ValidateShortcutKeys(Keys))
            {
                Reset();
            }
        }

        private void RenderText()
        {
            Text = keysHelper.GetShortcutString(Keys);
        }

        private bool ValidateShortcutKeys(Keys keyData)
        {
            var modifiers = keyData & Keys.ModifierMask;

            return modifiers != Keys.None && keysHelper.TryGetMainKey(keyData, out var _);
        }
    }
}
