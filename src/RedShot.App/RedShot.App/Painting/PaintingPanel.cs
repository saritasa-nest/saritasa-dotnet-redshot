using Eto.Drawing;
using Eto.Forms;
using RedShot.App.Painting.States;
using RedShot.Helpers.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.App.Painting
{
    internal class PaintingPanel : Panel
    {
        private NumericStepper drawSizeStepper;
        private ColorPicker colorPicker;

        public event EventHandler<PaintingState> StateChanged;

        public event EventHandler<double> DrawSizeChanged;

        public event EventHandler<Color> ColorChanged;

        public DefaultButton SaveImageButton { get; private set; }

        public DefaultButton PaintBackButton { get; private set; }

        public PaintingPanel()
        {
            BackgroundColor = Colors.WhiteSmoke;

            InitializeComponents();
            Content = GetContent();
        }

        private void InitializeComponents()
        {
            drawSizeStepper = new NumericStepper()
            {
                MinValue = 1,
                MaxValue = 30,
                Increment = 1,
                Width = 60
            };

            drawSizeStepper.ValueChanged += DrawSizeStepper_ValueChanged;

            colorPicker = new ColorPicker()
            {
                Width = 60,
                Height = 30,
                Value = Colors.Red,
            };

            colorPicker.ValueChanged += ColorPicker_ValueChanged;

            SaveImageButton = new DefaultButton("Save", 100, 40);

            PaintBackButton = new DefaultButton("Back", 100, 40);
        }

        private StackLayout GetContent()
        {
            return new StackLayout()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Orientation = Orientation.Horizontal,
                Items =
                {
                    colorPicker,
                    FormsHelper.VoidBox(10),
                    drawSizeStepper,
                    FormsHelper.VoidBox(10),
                    GetPaintingButtons(),
                    FormsHelper.VoidBox(20),
                    PaintBackButton,
                    FormsHelper.VoidBox(10),
                    SaveImageButton
                }
            };
        }

        private Control GetPaintingButtons()
        {
            var array = Enum.GetNames(typeof(PaintingState));

            var layout = new StackLayout()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Orientation = Orientation.Horizontal,
            };

            foreach (var state in array)
            {
                var button = new DefaultButton(state, 100, 40);
                button.Clicked += (o, e) => RequestStateChaging(state);

                layout.Items.Add(button);
                layout.Items.Add(FormsHelper.VoidBox(10));
            }

            return layout;
        }

        private void RequestStateChaging(string state)
        {
            StateChanged?.Invoke(this, Enum.Parse<PaintingState>(state));
        }

        private void ColorPicker_ValueChanged(object sender, EventArgs e)
        {
            ColorChanged?.Invoke(this, colorPicker.Value);
        }

        private void DrawSizeStepper_ValueChanged(object sender, EventArgs e)
        {
            DrawSizeChanged?.Invoke(this, drawSizeStepper.Value);
        }
    }
}
