using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut text box.
    /// </summary>
    internal class ShortcutTextBox : TextBox
    {
        private Keys keys;
        private readonly KeysParser keysParser;

        /// <summary>
        /// Hot keys.
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
        /// Constructor.
        /// </summary>
        public ShortcutTextBox()
        {
            Text = "None";
            keysParser = new KeysParser();
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

            if (e.Key == Keys.PrintScreen)
            {
                Keys = e.KeyData;
            }
        }

        private void RenderText()
        {
            Text = keysParser.GetShortcutString(Keys);
        }
    }
}
