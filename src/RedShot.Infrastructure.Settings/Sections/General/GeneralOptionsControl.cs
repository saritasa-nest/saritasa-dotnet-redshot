using System;
using Eto.Forms;
using Eto.Drawing;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Settings.Sections.General
{
    /// <summary>
    /// Format settings option dialog.
    /// </summary>
    internal class GeneralOptionsControl : Panel
    {
        private readonly GeneralOptions generalOptions;
        private CheckBox launchAtSystemStartCheckBox;
        private TextBox patternTextBox;
        private Label exampleLabel;
        private ComboBox updateIntervals;

        /// <summary>
        /// Active configuration data.
        /// </summary>
        public GeneralOptions GeneralOptions => generalOptions;

        /// <summary>
        /// Initializes format settings option dialog.
        /// </summary>
        public GeneralOptionsControl(GeneralOptions generalOptions)
        {
            this.generalOptions = generalOptions;
            InitializeComponents();
            BindControls();
        }

        /// <summary>
        /// Hack to remove focus from `updateIntervals` control.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            updateIntervals?.Focus();
            Focus();
            base.OnShown(e);
        }

        private void InitializeComponents()
        {
            exampleLabel = new Label();
            SetFormatExample(generalOptions.Pattern);

            patternTextBox = new TextBox()
            {
                Width = 300,
                Text = generalOptions.Pattern
            };
            patternTextBox.TextChanging += PatternTextBoxOnTextChanging;

            launchAtSystemStartCheckBox = new CheckBox
            {
                Text = "Launch at system start"
            };

            updateIntervals = new ComboBox()
            {
                Size = new Size(180, 19),
                ReadOnly = true
            };

            // External stack layout is needed to add padding. Padding doesn't work for GroupBox.
            Content = new StackLayout
            {
                Padding = 20,
                Spacing = 20,
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
                                    Padding = new Padding(10, 0),
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
                    launchAtSystemStartCheckBox,
                    GetUpdatingSettings(updateIntervals)
                }
            };
        }

        private static Control GetUpdatingSettings(ComboBox updateIntervals)
        {
            var label = new Label()
            {
                Text = "Update interval"
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    label,
                    updateIntervals
                }
            };
        }

        private void BindControls()
        {
            DataContext = generalOptions;

            launchAtSystemStartCheckBox.Bind(cb => cb.Checked, generalOptions, config => config.LaunchAtSystemStart);
            updateIntervals.BindWithEnum<UpdateInterval>().BindDataContext((GeneralOptions o) => o.UpdateInterval);
        }

        private void PatternTextBoxOnTextChanging(object sender, TextChangingEventArgs e)
        {
            generalOptions.Pattern = e.NewText;
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
