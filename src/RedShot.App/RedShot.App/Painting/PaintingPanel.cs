using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.App.Painting.States;
using RedShot.Helpers.Forms;
using RedShot.App.Properties;

namespace RedShot.App.Painting
{
    internal class PaintingPanel : Panel
    {
        private NumericStepper drawSizeStepper;
        private ColorPicker colorPicker;

        public event EventHandler<PaintingState> StateChanged;

        public event EventHandler<double> DrawSizeChanged;

        public event EventHandler<Color> ColorChanged;

        public ImageButton PointsEnableButton { get; private set; }

        public ImageButton RectangleEnableButton { get; private set; }

        public ImageButton SaveImageButton { get; private set; }

        public ImageButton PaintBackButton { get; private set; }

        public PaintingPanel()
        {
            InitializeComponents();
            Content = GetContent();

            MinimumSize = new Size(500, 50);
        }

        private void InitializeComponents()
        {
            drawSizeStepper = new NumericStepper()
            {
                MinValue = 1,
                MaxValue = 30,
                Increment = 1,
                Width = 60,
                Height = 40
            };

            drawSizeStepper.ValueChanged += DrawSizeStepper_ValueChanged;

            colorPicker = new ColorPicker()
            {
                Width = 60,
                Height = 30,
                Value = Colors.Red,
            };

            colorPicker.ValueChanged += ColorPicker_ValueChanged;

            var buttonSize = new Size(70, 40);
            var imageSize = new Size(25, 25);

            var paintImage = new Bitmap(Resources.paint_brush);
            var saveImage = new Bitmap(Resources.upload);
            var backImage = new Bitmap(Resources.back);
            var rectangleImage = new Bitmap(Resources.rectangle);

            PointsEnableButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize);
            RectangleEnableButton = new ImageButton(buttonSize, rectangleImage, scaleImageSize: imageSize);
            SaveImageButton = new ImageButton(buttonSize, saveImage, scaleImageSize: imageSize);
            PaintBackButton = new ImageButton(buttonSize, backImage, scaleImageSize: imageSize);

            PointsEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Points);
            RectangleEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Rectangle);
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
                    FormsHelper.VoidBox(10),
                    colorPicker,
                    FormsHelper.VoidBox(10),
                    drawSizeStepper,
                    FormsHelper.VoidBox(10),
                    PointsEnableButton,
                    FormsHelper.VoidBox(10),
                    RectangleEnableButton,
                    FormsHelper.VoidBox(20),
                    PaintBackButton,
                    FormsHelper.VoidBox(10),
                    SaveImageButton
                }
            };
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
