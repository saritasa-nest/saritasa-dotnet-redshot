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
            SetFormatExample(configurationOption.Pattern);

            patternTextBox = new TextBox()
            {
                Width = 300,
                Text = configurationOption.Pattern
            };
            patternTextBox.TextChanging += PatternTextBoxOnTextChanging;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = 20,
                Spacing = 5,
                Items =
                {
                    new Label()
                    {
                        Text = "Pattern"
                    },
                    patternTextBox,
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
                        Text = "File name template",
                        Content = new FilenameTemplatePanel(pattern => patternTextBox.Text += pattern)
                    }
                }
            };
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
