using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Painting.PaintingActions.TextInput
{
    /// <summary>
    /// Text input view.
    /// </summary>
    internal class TextInputView : Form
    {
        private readonly IPaintingAction textPaintingAction;

        private bool saveText;
        private ColorPicker textColorPicker;
        private FontPicker textFontPicker;
        private DefaultButton okButton;
        private TextArea textArea;

        /// <summary>
        /// Initializes text input dialog.
        /// </summary>
        public TextInputView(IPaintingAction textPaintingAction)
        {
            this.textPaintingAction = textPaintingAction;
            Title = "Text input dialog";
            InitializeComponents();
            this.Closed += OnClosed;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (saveText == false)
            {
                var action = new TextInputAction(string.Empty, textFontPicker.Value, textColorPicker.Value);
                textPaintingAction.InputUserAction(action);
            }
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
                Value = Fonts.Fantasy(14)
            };
            textFontPicker.ValueChanged += TextOptionsChanged;

            okButton = new DefaultButton("OK", 80, 30);
            okButton.Clicked += OkButtonOnClicked;

            textArea = new TextArea()
            {
                Size = new Size(300, 200),
                AllowDrop = true
            };
            textArea.TextChanged += TextAreaOnTextChanged;

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

        private void TextAreaOnTextChanged(object sender, EventArgs e)
        {
            var action = new TextInputAction(textArea.Text, textFontPicker.Value, textColorPicker.Value);
            textPaintingAction.InputUserAction(action);
        }

        private void OkButtonOnClicked(object sender, EventArgs e)
        {
            saveText = true;
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