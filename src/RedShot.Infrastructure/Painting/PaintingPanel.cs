using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Painting.States;

namespace RedShot.Infrastructure.Painting
{
    public class PaintingPanel : Panel
    {
        private NumericStepper drawSizeStepper;
        private ColorPicker colorPicker;

        public event EventHandler<PaintingState> StateChanged;

        public event EventHandler<double> DrawSizeChanged;

        public event EventHandler<Color> ColorChanged;

        public ImageButton PointsEnableButton { get; private set; }

        public ImageButton RectangleEnableButton { get; private set; }

        public ImageButton EraseEnableButton { get; private set; }

        public ImageButton SaveImageButton { get; private set; }

        public ImageButton PaintBackButton { get; private set; }

        public PaintingPanel()
        {
            InitializeComponents();
            Content = GetContent();

            MinimumSize = new Size(500, 40);
        }

        private void InitializeComponents()
        {
            drawSizeStepper = new NumericStepper()
            {
                MinValue = 1,
                MaxValue = 30,
                Increment = 1,
                Width = 60,
                Height = 30,
                ToolTip = "Size of drawing line"
            };

            drawSizeStepper.ValueChanged += DrawSizeStepper_ValueChanged;

            colorPicker = new ColorPicker()
            {
                Width = 60,
                Height = 30,
                Value = Colors.Red,
                ToolTip = "Pick a color for drawing"
            };

            colorPicker.ValueChanged += ColorPicker_ValueChanged;

            var buttonSize = new Size(60, 30);
            var imageSize = new Size(20, 20);

            var paintImage = new Bitmap(Resources.Properties.Resources.Paintbrush);
            var saveImage = new Bitmap(Resources.Properties.Resources.Upload);
            var backImage = new Bitmap(Resources.Properties.Resources.Back);
            var rectangleImage = new Bitmap(Resources.Properties.Resources.Rectangle);
            var eraseImage = new Bitmap(Resources.Properties.Resources.EraseIcon);

            PointsEnableButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize)
            {
                ToolTip = "Draw a line of any shape"
            };

            RectangleEnableButton = new ImageButton(buttonSize, rectangleImage, scaleImageSize: imageSize)
            {
                ToolTip = "Draw a rectangle"
            };

            SaveImageButton = new ImageButton(buttonSize, saveImage, scaleImageSize: imageSize)
            {
                ToolTip = "Upload the image"
            };

            PaintBackButton = new ImageButton(buttonSize, backImage, scaleImageSize: imageSize)
            {
                ToolTip = "Take a step back"
            };

            EraseEnableButton = new ImageButton(buttonSize, eraseImage, scaleImageSize: imageSize)
            {
                ToolTip = "Erase lines from image"
            };

            PointsEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Points);
            RectangleEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Rectangle);
            EraseEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Erase);
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
                    FormsHelper.VoidBox(10),
                    EraseEnableButton,
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
