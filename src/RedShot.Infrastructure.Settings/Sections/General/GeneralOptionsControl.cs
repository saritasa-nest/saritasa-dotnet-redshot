﻿using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Formatting;

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
                Text = "Update Interval"
            };

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
                    },
                    new StackLayout
                    {
                        Orientation = Orientation.Vertical,
                        Padding = 10,
                        Items =
                        {
                            new Label()
                            {
                                Text = "Update Interval"
                            },
                            updateIntervals
                        }
                    },
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
