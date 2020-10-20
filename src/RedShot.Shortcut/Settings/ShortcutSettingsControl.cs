using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut settings control.
    /// </summary>
    internal class ShortcutSettingsControl : Panel
    {
        private readonly List<Shortcuts.Shortcut> shortcuts;

        /// <summary>
        /// Create the control.
        /// </summary>
        public ShortcutSettingsControl(List<Shortcuts.Shortcut> shortcuts)
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

#if _UNIX
            stackLayout.Items.Add(new Label()
            {
                TextColor = Colors.Red,
                Text = "Shortcuts are not supported on this platform!"
            });
#endif
            Content = stackLayout;
        }

        private Control GetShortcutStack(Shortcuts.Shortcut shortcut)
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
