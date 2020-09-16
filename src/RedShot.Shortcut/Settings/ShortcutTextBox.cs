using Eto.Forms;

namespace RedShot.Shortcut.Settings
{
    internal class ShortcutTextBox : TextBox
    {
        private Keys keys;

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

        public ShortcutTextBox()
        {
            Text = "None";
        }

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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RenderText()
        {
            Text = Keys.ToShortcutString();
        }

        public void Reset()
        {
            Keys = Keys.None;
            Text = "None";
        }
    }
}
