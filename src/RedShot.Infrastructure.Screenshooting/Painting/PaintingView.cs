using System;
using System.ComponentModel;
using SkiaSharp;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Screenshooting.Painting.States;
using RedShot.Resources;

namespace RedShot.Infrastructure.Screenshooting.Painting
{
    /// <summary>
    /// Painting view.
    /// </summary>
    internal class PaintingView : Form
    {
        private readonly int paintingPanelWidth = 60;
        private readonly Bitmap image;
        private PaintingPanel paintingPanel;
        private ImagePanel imagePanel;
        private SKPaint paint;
        private int uploadedImageHash;

        /// <summary>
        /// Initializes painting view via image.
        /// </summary>
        public PaintingView(Bitmap image)
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = "Image editor";
            MinimumSize = new Size(500, paintingPanelWidth);
            Resizable = false;
            this.image = image;
            InitializeComponents();
            Content = GetContent();

            this.Shown += PaintingViewShown;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            imagePanel.TextInputView?.Close();

            if (uploadedImageHash == imagePanel.GetImageHash())
            {
                return;
            }

            const string message = "Do you want to close the editor without uploading the picture?";
            const string title = "RedShot question";
            var dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo,
                MessageBoxType.Question);

            if (dialogResult != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void PaintingViewShown(object sender, EventArgs e)
        {
            Location = ScreenHelper.GetCenterLocation(Size);
        }

        private void InitializeComponents()
        {
            paint = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = 1,
                Style = SKPaintStyle.Stroke,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            imagePanel = new ImagePanel(image);
            imagePanel.ChangePaint(paint);

            paintingPanel = new PaintingPanel();

            paintingPanel.DrawSizeChanged += PaintingPanelDrawSizeChanged;
            paintingPanel.ColorChanged += PaintingPanelColorChanged;
            paintingPanel.StateChanged += PaintingPanelStateChanged;
            paintingPanel.SaveImageButton.Clicked += SaveImageButtonClicked;
            paintingPanel.PaintBackButton.Clicked += PaintBackButtonClicked;
        }

        private void PaintBackButtonClicked(object sender, EventArgs e)
        {
            imagePanel.PaintBack();
        }

        private void SaveImageButtonClicked(object sender, EventArgs e)
        {
            var bitmap = imagePanel.ScreenShot();
            ScreenshotManager.UploadScreenShot(bitmap);
            uploadedImageHash = imagePanel.GetImageHash();
        }

        private void PaintingPanelStateChanged(object sender, PaintingState state)
        {
            imagePanel.ChangePaintingState(state);
        }

        private void PaintingPanelColorChanged(object sender, Color color)
        {
            paint.Color = SkiaSharpHelper.GetSKColorFromEtoColor(color);
            imagePanel.ChangePaint(paint);
        }

        private void PaintingPanelDrawSizeChanged(object sender, double size)
        {
            paint.StrokeWidth = (float)size;
            paint.TextSize = (float)size;

            imagePanel.ChangePaint(paint);
        }

        private StackLayout GetContent()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 1,
                Items =
                {
                    paintingPanel,
                    GetImageArea()
                }
            };
        }

        private Control GetImageArea()
        {
            return new StackLayout()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Vertical,
                Items =
                {
                    imagePanel
                }
            };
        }
    }
}
