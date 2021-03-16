using Eto.Drawing;
using Eto.Forms;
using System.Collections.Generic;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Resources;
using System.Linq;
using RedShot.Shortcut.Units;

namespace RedShot.Infrastructure.Settings.Sections.Shortcut
{
    /// <summary>
    /// Shortcut options control.
    /// </summary>
    internal class ShortcutOptionsControl : Panel
    {
        private readonly IEnumerable<BaseShortcut> shortcuts;

        /// <summary>
        /// Create the control.
        /// </summary>
        public ShortcutOptionsControl(IEnumerable<BaseShortcut> shortcuts)
        {
            this.shortcuts = GetOrderedShortcuts(shortcuts.ToList());
            InitializeComponents();
        }

        private IEnumerable<BaseShortcut> GetOrderedShortcuts(IEnumerable<BaseShortcut> shortcuts)
        {
            var screenshotShortcut = shortcuts.First(s => s is ScreenShotShortcut);
            var ordered = new List<BaseShortcut>();
            ordered.Add(screenshotShortcut);
            ordered.AddRange(shortcuts.Where(s => s != screenshotShortcut));

            return ordered;
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

        private Control GetShortcutStack(BaseShortcut shortcut)
        {
            var shortcutTextBox = new ShortcutTextBox()
            {
                Size = new Size(200, 22),
                Keys = shortcut.Keys
            };
            shortcutTextBox.KeysChanged += (o, e) =>
            {
                if (e.Keys == Keys.None)
                {
                    shortcut.Keys = Keys.None;
                    return;
                }

                // This logic prevent using shortcut keys for the second time.
                if (shortcuts.Any(s => s.Keys == e.Keys && s != shortcut))
                {
                    shortcutTextBox.Reset();
                }
                else
                {
                    shortcut.Keys = e.Keys;
                }
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
