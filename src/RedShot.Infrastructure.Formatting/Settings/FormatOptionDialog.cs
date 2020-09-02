using System;
using System.ComponentModel;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Formatting.Settings
{
    /// <summary>
    /// Format settings option dialog.
    /// </summary>
    internal class FormatOptionDialog : Dialog<DialogResult>
    {
        private readonly FormatConfigurationOption configurationOption;
        private TextBox patternTextBox;
        private Label exampleLabel;
        private DefaultButton okButton;
        private DefaultButton guideButton;

        /// <summary>
        /// Initializes format settings option dialog.
        /// </summary>
        public FormatOptionDialog(FormatConfigurationOption configurationOption)
        {
            Title = "Format link options";
            this.configurationOption = configurationOption;
            InitializeComponents();
            this.Closing += OnClosing;
            Resizable = true;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (Result == DialogResult.Ok)
            {
                if (string.IsNullOrEmpty(patternTextBox.Text) || string.IsNullOrWhiteSpace(patternTextBox.Text))
                {
                    var dialog = new YesNoDialog()
                    {
                        Message = "The format link is empty. Do you want to change it?",
                        Size = new Size(300, 200)
                    };

                    using (dialog)
                    {
                        if (dialog.ShowModal(this) == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                configurationOption.Pattern = patternTextBox.Text;
            }
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
            SetFormatExample(configurationOption.Pattern);
            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += SaveButtonOnClicked;
            guideButton = new DefaultButton("Guide", 60, 25);
            guideButton.Clicked += GuideButtonOnClicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 20,
                Items =
                {
                    new Label()
                    {
                        Text = "Pattern"
                    },
                    FormsHelper.VoidBox(5),
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Items =
                        {
                            patternTextBox,
                            FormsHelper.VoidBox(10),
                            guideButton
                        }
                    },
                    FormsHelper.VoidBox(5),
                    exampleLabel,
                    FormsHelper.VoidBox(15),
                    okButton
                }
            };
        }

        private void GuideButtonOnClicked(object sender, EventArgs e)
        {
            using (var guideDialog = new FormatGuideDialog())
            {
                guideDialog.ShowModal(this);
            }
        }

        private void SaveButtonOnClicked(object sender, EventArgs e)
        {
            Result = DialogResult.Ok;
            Close();
        }

        private void PatternTextBoxOnTextChanging(object sender, TextChangingEventArgs e)
        {
            SetFormatExample(e.NewText);
        }

        private void SetFormatExample(string pattern)
        {
            var lineSize = 45;
            
            var text = exampleLabel.Text = FormatManager.GetFormattedName(pattern);

            text = BreakLine(text, 45);

            exampleLabel.Text = text;
        }

        private string BreakLine(string line, int lineSize)
        {
            var brokenLine = string.Empty;
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