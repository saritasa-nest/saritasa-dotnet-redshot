using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Painting.States;

namespace RedShot.Infrastructure.Painting
{
    /// <summary>
    /// Painting panel.
    /// Provides managing buttons for painting view.
    /// </summary>
    internal class PaintingPanel : Panel
    {
        private NumericStepper drawSizeStepper;
        private ColorPicker colorPicker;

        /// <summary>
        /// State changed event.
        /// Invokes when painting state changes.
        /// </summary>
        public event EventHandler<PaintingState> StateChanged;

        /// <summary>
        /// Draw size changed event.
        /// Invokes when draw size was changed.
        /// </summary>
        public event EventHandler<double> DrawSizeChanged;

        /// <summary>
        /// Draw size changed event.
        /// Invokes when draw size was changed.
        /// </summary>
        public event EventHandler<Color> ColorChanged;

        /// <summary>
        /// Button for enabling painting line action.
        /// </summary>
        public ImageButton PointsEnableButton { get; private set; }

        /// <summary>
        /// Button for enabling painting rectangle action.
        /// </summary>
        public ImageButton RectangleEnableButton { get; private set; }

        /// <summary>
        /// Button for enabling painting arrow action.
        /// </summary>
        public ImageButton ArrowEnableButton { get; private set; }
        
        /// <summary>
        /// Button for enabling painting arrow action.
        /// </summary>
        public DefaultButton TextEnableButton { get; private set; }

        /// <summary>
        /// Button for enabling erasing action.
        /// </summary>
        public ImageButton EraseEnableButton { get; private set; }

        /// <summary>
        /// Button for save drawing image.
        /// </summary>
        public ImageButton SaveImageButton { get; private set; }

        /// <summary>
        /// Button for moving back.
        /// </summary>
        public ImageButton PaintBackButton { get; private set; }

        /// <summary>
        /// Initializes painting panel.
        /// </summary>
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
                ToolTip = "Size of painting line"
            };

            drawSizeStepper.ValueChanged += DrawSizeStepper_ValueChanged;

            colorPicker = new ColorPicker()
            {
                Width = 60,
                Height = 30,
                Value = Colors.Red,
                ToolTip = "Pick a color for painting"
            };

            colorPicker.ValueChanged += ColorPicker_ValueChanged;

            var buttonSize = new Size(60, 30);
            var imageSize = new Size(20, 20);

            var paintImage = new Bitmap(Resources.Properties.Resources.Paintbrush);
            var saveImage = new Bitmap(Resources.Properties.Resources.Upload);
            var backImage = new Bitmap(Resources.Properties.Resources.Back);
            var rectangleImage = new Bitmap(Resources.Properties.Resources.Rectangle);
            var eraseImage = new Bitmap(Resources.Properties.Resources.EraseIcon);
            var arrowImage = new Bitmap(Resources.Properties.Resources.Arrow);

            PointsEnableButton = new ImageButton(buttonSize, paintImage, scaleImageSize: imageSize)
            {
                ToolTip = "Paint a line of any shape"
            };

            RectangleEnableButton = new ImageButton(buttonSize, rectangleImage, scaleImageSize: imageSize)
            {
                ToolTip = "Paint a rectangle"
            };

            SaveImageButton = new ImageButton(buttonSize, saveImage, scaleImageSize: imageSize)
            {
                ToolTip = "Upload the picture"
            };

            PaintBackButton = new ImageButton(buttonSize, backImage, scaleImageSize: imageSize)
            {
                ToolTip = "Take a step back"
            };

            EraseEnableButton = new ImageButton(buttonSize, eraseImage, scaleImageSize: imageSize)
            {
                ToolTip = "Erase paintings from the picture"
            };

            ArrowEnableButton = new ImageButton(buttonSize, arrowImage, scaleImageSize: imageSize)
            {
                ToolTip = "Paint an arrow"
            };

            TextEnableButton = new DefaultButton("Text", buttonSize.Width, buttonSize.Height);

            TextEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Text);
            PointsEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Points);
            RectangleEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Rectangle);
            EraseEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Erase);
            ArrowEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, PaintingState.Arrow);
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
                    TextEnableButton,
                    FormsHelper.VoidBox(10),
                    PointsEnableButton,
                    FormsHelper.VoidBox(10),
                    RectangleEnableButton,
                    FormsHelper.VoidBox(10),
                    ArrowEnableButton,
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
