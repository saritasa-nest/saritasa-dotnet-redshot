using Eto.Drawing;
using Eto.Forms;
using RedShot.Shortcuts;
using System.Collections.Generic;

namespace RedShot.Shortcut.Settings
{
    internal class ShortcutSettingsControl : Panel
    {
        List<IShortcut> shortcuts;

        public ShortcutSettingsControl(List<IShortcut> shortcuts)
        {
            this.shortcuts = shortcuts;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var stackLayout = new StackLayout()
            {
                Padding = 50,
                Spacing = 15
            };

            shortcuts.ForEach((s) => stackLayout.Items.Add(GetShortcutStack(s)));
            Content = stackLayout;
        }

        private Control GetShortcutStack(IShortcut shortcut)
        {
            var shortcutTextBox = new ShortcutTextBox()
            {
                Size = new Size(200, 22)
            };
            shortcutTextBox.TextChanging += (o, e) =>
            {
                shortcut.Keys = shortcutTextBox.Keys;
            };

            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    new Label()
                    {
                        Text = shortcut.Name
                    },
                    shortcutTextBox
                }
            };
        }
    }
}
