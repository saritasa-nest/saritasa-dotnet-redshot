using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Resources;
using System.Linq;

namespace RedShot.Shortcut.Settings
{
    /// <summary>
    /// Shortcut settings control.
    /// </summary>
    internal class ShortcutSettingsControl : Panel
    {
        private readonly IEnumerable<Shortcuts.Shortcut> shortcuts;

        /// <summary>
        /// Create the control.
        /// </summary>
        public ShortcutSettingsControl(IEnumerable<Shortcuts.Shortcut> shortcuts)
        {
            this.shortcuts = shortcuts;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var layout = new TableLayout()
            {
                Padding = new Padding(20, 20, 0, 0),
                Spacing = new Size(15, 20)
            };

            foreach (var shortcut in shortcuts)
            {
                layout.Rows.Add(new TableRow(new TableCell(GetShortcutStack(shortcut))));
            }

#if _UNIX
            layout.Rows.Add(new TableRow(new TableCell(new Label()
            {
                TextColor = Colors.Red,
                Text = "Shortcuts are not supported on this platform!"
            })));
#endif
            Content = layout;
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

            var clearButton = new ImageButton(new Size(26, 24), Icons.Close, scaleImageSize: new Size(13, 12))
            {
                ToolTip = "Clear"
            };

            clearButton.Clicked += (o, e) => shortcutTextBox.Reset();

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    new Label()
                    {
                        Text = shortcut.Name
                    },
                    new StackLayout()
                    {
                        Spacing = 10,
                        Padding = new Padding(3, 3, 0, 0),
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Items =
                        {
                            shortcutTextBox,
                            clearButton
                        }
                    }
                }
            };
        }
    }
}
