using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut textbox.
    /// </summary>
    internal class ShortcutTextBox : TextBox
    {
        private Keys keys;

        /// <summary>
        /// Hotkeys.
        /// </summary>
        public Keys Keys
        {
            get { return keys; }

            set
            {
                keys = value;

                if (keys != Keys.None)
                {
                    RenderText();
                }
            }
        }

        /// <summary>
        /// Create the text box.
        /// </summary>
        public ShortcutTextBox()
        {
            Text = "None";

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

            if (!Keys.HasFlag(e.KeyData))
            {
                Keys = e.KeyData;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RenderText()
        {
            Text = Keys.ToShortcutString();
        }
    }
}
