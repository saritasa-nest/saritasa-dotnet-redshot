using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option dialog.
    /// </summary>
    internal class GeneralOptionControl : Panel
    {
        private readonly GeneralConfigurationOption configurationOption;
        private CheckBox launchAtSystemStartCheckBox;
        private TextBox patternTextBox;
        private Label exampleLabel;

        /// <summary>
        /// Initializes format settings option dialog.
        /// </summary>
        public GeneralOptionControl(GeneralConfigurationOption configurationOption)
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

            launchAtSystemStartCheckBox = new CheckBox
            {
                Text = "Launch at system start"
            };
            launchAtSystemStartCheckBox.Bind(cb => cb.Checked, configurationOption, config => config.LaunchAtSystemStart);

            // External stack layout is needed to add padding. Padding doesn't work for GroupBox.
            Content = new StackLayout
            {
                Padding = 5,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Items =
                {
                    new GroupBox()
                    {
                        Text = "File name template",
                        Content = new StackLayout
                        {
                            Items =
                            {
                                new StackLayout()
                                {
                                    Orientation = Orientation.Vertical,
                                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                                    Padding = 10,
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
                                    }
                                },
                                new FilenameTemplatePanel(pattern => patternTextBox.Text += pattern)
                            }
                        }
                    },
                    new StackLayout
                    {
                        Padding = 10,
                        Items =
                        {
                            launchAtSystemStartCheckBox
                        }
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
