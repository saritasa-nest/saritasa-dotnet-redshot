using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option dialog.
    /// </summary>
    internal class FormatOptionControl : Panel
    {
        private readonly FormatConfigurationOption configurationOption;
        private TextBox patternTextBox;
        private Label exampleLabel;
        private DefaultButton addButton;
        private ContextMenu formatItemsMenu;

        /// <summary>
        /// Initializes format settings option dialog.
        /// </summary>
        public FormatOptionControl(FormatConfigurationOption configurationOption)
        {
            this.configurationOption = configurationOption;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            formatItemsMenu = GetFormatItemsMenu();
            exampleLabel = new Label();
            SetFormatExample(configurationOption.Pattern);

            patternTextBox = new TextBox()
            {
                Width = 300,
                Text = configurationOption.Pattern
            };
            patternTextBox.TextChanging += PatternTextBoxOnTextChanging;

            addButton = new DefaultButton("Add", 60, 25)
            {
                ToolTip = "Add format item"
            };
            addButton.Clicked += AddButtonClicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 20,
                Spacing = 5,
                Items =
                {
                    new Label()
                    {
                        Text = "Pattern"
                    },
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 10,
                        Items =
                        {
                            patternTextBox,
                            addButton
                        }
                    },
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 5,
                        Items =
                        {
                            new Label()
                            {
                                Text = "Example:"
                            },
                            exampleLabel
                        }
                    },
                    FormsHelper.GetVoidBox(20),
                    new GroupBox()
                    {
                        Text = "Guide",
                        Content = new FormatGuidePanel()
                    }
                }
            };
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            var location = new Point(addButton.Location.X + addButton.Width, addButton.Location.Y);
            formatItemsMenu.Show((Control)sender, location);
        }

        private ContextMenu GetFormatItemsMenu()
        {
            var formatItemsMenu = new ContextMenu();

            foreach (var formatItem in FormatManager.FormatItems)
            {
                formatItemsMenu.Items.Add(GetFormatItemButton(formatItem.Name, formatItem.Pattern));
            }

            formatItemsMenu.Items.AddSeparator();

            formatItemsMenu.Items.Add(GetFormatItemButton("Custom text", "[your_text]"));

            return formatItemsMenu;
        }

        private ButtonMenuItem GetFormatItemButton(string name, string pattern)
        {
            var fullPattern = GetFullPattern(pattern);

            var button = new ButtonMenuItem()
            {
                Text = $"{name}"
            };
            button.Click += (o, e) =>
            {
                patternTextBox.Text += fullPattern;
            };

            return button;
        }

        private string GetFullPattern(string pattern)
        {
            return $"{FormatManager.FormatTag}{pattern}";
        }

        private void PatternTextBoxOnTextChanging(object sender, TextChangingEventArgs e)
        {
            configurationOption.Pattern = e.NewText;
            SetFormatExample(e.NewText);
        }

        private void SetFormatExample(string pattern)
        {
            if (FormatManager.TryFormat(pattern, out var result))
            {
                exampleLabel.Text = result;
            }
            else
            {
                exampleLabel.Text = "Invalid pattern";
            }
        }
    }
}
