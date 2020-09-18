using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using RedShot.Shortcut.Shortcuts;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut settings control.
    /// </summary>
    internal class ShortcutSettingsControl : Panel
    {
        private readonly List<IShortcut> shortcuts;

        /// <summary>
        /// Create the control.
        /// </summary>
        public ShortcutSettingsControl(List<IShortcut> shortcuts)
        {
            this.shortcuts = shortcuts;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var stackLayout = new StackLayout()
            {
                Padding = 30,
                Spacing = 15
            };

            shortcuts.ForEach((s) => stackLayout.Items.Add(GetShortcutStack(s)));
            Content = stackLayout;
        }

        private Control GetShortcutStack(IShortcut shortcut)
        {
            var shortcutTextBox = new ShortcutTextBox()
            {
                Size = new Size(200, 22),
                Keys = shortcut.Keys
            };
            shortcutTextBox.TextChanging += (o, e) =>
            {
                shortcut.Keys = shortcutTextBox.Keys;
            };

            return FormsHelper.GetBaseStack(shortcut.Name, shortcutTextBox);
        }
    }
}
