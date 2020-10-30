using System;
using Eto.Drawing;
using Eto.Forms;
using Prism.Events;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Screenshooting.Painting.States;
using RedShot.Resources;

namespace RedShot.Infrastructure.Screenshooting.Painting
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
        public event EventHandler<DataEventArgs<PaintingState>> StateChanged;

        /// <summary>
        /// Draw size changed event.
        /// Invokes when draw size was changed.
        /// </summary>
        public event EventHandler<DataEventArgs<double>> DrawSizeChanged;

        /// <summary>
        /// Draw size changed event.
        /// Invokes when draw size was changed.
        /// </summary>
        public event EventHandler<DataEventArgs<Color>> ColorChanged;

        /// <summary>
        /// Button for enabling painting line action.
        /// </summary>
        public ImageButton BrushEnableButton { get; private set; }

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
        public ImageButton TextEnableButton { get; private set; }

        /// <summary>
        /// Button for enabling erasing action.
        /// </summary>
        public ImageButton EraseEnableButton { get; private set; }

        /// <summary>
        /// Button for save drawing image.
        /// </summary>
        public UploadingButton UploadImageButton { get; private set; }

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
                ToolTip = "Size of painting line",
                Value = 3
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

            BrushEnableButton = new ImageButton(buttonSize, Icons.PaintBrush, scaleImageSize: imageSize)
            {
                ToolTip = "Brush"
            };

            RectangleEnableButton = new ImageButton(buttonSize, Icons.Rectangle, scaleImageSize: new Size(23, 20))
            {
                ToolTip = "Rectangle"
            };

            UploadImageButton = new UploadingButton();

            PaintBackButton = new ImageButton(buttonSize, Icons.Back, scaleImageSize: imageSize)
            {
                ToolTip = "Undo"
            };

            EraseEnableButton = new ImageButton(buttonSize, Icons.EraseIcon, scaleImageSize: imageSize)
            {
                ToolTip = "Eraser"
            };

            ArrowEnableButton = new ImageButton(buttonSize, Icons.Arrow, scaleImageSize: imageSize)
            {
                ToolTip = "Arrow"
            };

            TextEnableButton = new ImageButton(buttonSize, Icons.Text, scaleImageSize: imageSize)
            {
                ToolTip = "Enter text"
            };

            TextEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, new DataEventArgs<PaintingState>(PaintingState.Text));
            BrushEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, new DataEventArgs<PaintingState>(PaintingState.Brush));
            RectangleEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, new DataEventArgs<PaintingState>(PaintingState.Rectangle));
            EraseEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, new DataEventArgs<PaintingState>(PaintingState.Erase));
            ArrowEnableButton.Clicked += (o, e) => StateChanged?.Invoke(this, new DataEventArgs<PaintingState>(PaintingState.Arrow));
        }

        private StackLayout GetContent()
        {
            return new StackLayout()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Orientation = Orientation.Horizontal,
                Padding = new Padding(10, 0),
                Spacing = 10,
                Items =
                {
                    colorPicker,
                    drawSizeStepper,
                    TextEnableButton,
                    BrushEnableButton,
                    RectangleEnableButton,
                    ArrowEnableButton,
                    EraseEnableButton,
                    FormsHelper.GetVoidBox(10),
                    PaintBackButton,
                    UploadImageButton
                }
            };
        }

        private void ColorPicker_ValueChanged(object sender, EventArgs e)
        {
            ColorChanged?.Invoke(this, new DataEventArgs<Color>(colorPicker.Value));
        }

        private void DrawSizeStepper_ValueChanged(object sender, EventArgs e)
        {
            DrawSizeChanged?.Invoke(this, new DataEventArgs<double>(drawSizeStepper.Value));
        }
    }
}
