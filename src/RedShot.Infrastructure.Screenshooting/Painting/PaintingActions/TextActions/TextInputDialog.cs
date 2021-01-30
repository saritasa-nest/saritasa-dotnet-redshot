using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextActions
{
    /// <summary>
    /// Text input view.
    /// </summary>
    internal class TextInputDialog : Dialog<TextDialogResult>
    {
        private ColorPicker textColorPicker;
        private FontPicker textFontPicker;
        private DefaultButton okButton;
        private TextArea textArea;

        /// <summary>
        /// Initializes text input dialog.
        /// </summary>
        public TextInputDialog()
        {
            Title = "Enter Text";
            InitializeComponents();
            this.Shown += TextInputViewShown;
            this.KeyDown += TextInputDialogKeyDown;

            this.Resizable = false;
            this.Maximizable = false;

            Result = new TextDialogResult();
        }

        private void TextInputViewShown(object sender, EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            textColorPicker = new ColorPicker()
            {
                Size = new Size(60, 30),
                Value = Colors.Red
            };
            textColorPicker.ValueChanged += TextOptionsChanged;

            textFontPicker = new FontPicker()
            {
                Size = new Size(200, 30),
                Value = Fonts.Fantasy(25)
            };
            textFontPicker.ValueChanged += TextOptionsChanged;

            okButton = new DefaultButton("OK", 80, 30);
            okButton.Clicked += OkButtonOnClicked;

            textArea = new TextArea()
            {
                Size = new Size(300, 200),
                AllowDrop = true
            };
            textArea.KeyDown += TextInputDialogKeyDown;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 20,
                Spacing = 10,
                Items =
                {
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 10,
                        Items =
                        {
                            textColorPicker,
                            textFontPicker
                        }
                    },
                    textArea,
                    okButton
                }
            };
            SetTextOptions();
        }

        private void TextInputDialogKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                Result.DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void OkButtonOnClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textArea.Text))
            {
                var action = new TextInputAction(textArea.Text, textFontPicker.Value, textColorPicker.Value);
                Result = new TextDialogResult()
                {
                    DialogResult = DialogResult.Ok,
                    TextInputAction = action
                };
            }

            Close();
        }

        private void TextOptionsChanged(object sender, EventArgs e)
        {
            SetTextOptions();
        }

        private void SetTextOptions()
        {
            textArea.Font = textFontPicker.Value;
            textArea.TextColor = textColorPicker.Value;
            Invalidate(true);
        }
    }
}