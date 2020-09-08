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
        private DefaultButton guideButton;
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
            exampleLabel = new Label();

            patternTextBox = new TextBox()
            {
                Width = 300,
                Text = configurationOption.Pattern
            };
            patternTextBox.TextChanging += PatternTextBoxOnTextChanging;
            patternTextBox.MouseDown += PatternTextBoxMouseDown;

            guideButton = new DefaultButton("Guide", 60, 25)
            {
                ToolTip = "Open guide view"
            };
            guideButton.Clicked += GuideButtonOnClicked;

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
                            guideButton
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
                }
            };
        }

        private void PatternTextBoxMouseDown(object sender, MouseEventArgs e)
        {
            var location = new Point(patternTextBox.Location.X + patternTextBox.Width, patternTextBox.Location.Y);
            GetFormatItemsMenu().Show((Control)sender, location);
            patternTextBox.Focus();
        }

        private void GuideButtonOnClicked(object sender, EventArgs e)
        {
            using (var guideDialog = new FormatGuideDialog())
            {
                guideDialog.ShowModal(this);
            }
        }

        private ContextMenu GetFormatItemsMenu()
        {
            if (formatItemsMenu != null)
            {
                return formatItemsMenu;
            }

            formatItemsMenu = new ContextMenu();

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
                exampleLabel.Text = BreakLine(result, 45);
            }
            else
            {
                exampleLabel.Text = "Invalid pattern";
            }
        }

        private string BreakLine(string line, int lineSize)
        {
            string brokenLine;
            if (line.Length > lineSize)
            {
                brokenLine = line.Substring(0, lineSize) + "\r\n";
                brokenLine += BreakLine(line.Substring(lineSize, line.Length - lineSize), lineSize);
            }
            else
            {
                brokenLine = line;
            }

            return brokenLine;
        }
    }
}
