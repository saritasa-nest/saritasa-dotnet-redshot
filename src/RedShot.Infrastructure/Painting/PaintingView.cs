using System;
using System.ComponentModel;
using SkiaSharp;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Painting.States;

namespace RedShot.Infrastructure.Painting
{
    /// <summary>
    /// Painting view.
    /// </summary>
    internal class PaintingView : Form
    {
        private int paintingPanelWidth = 60;
        private readonly Bitmap image;
        private PaintingPanel paintingPanel;
        private ImagePanel imagePanel;
        private SKPaint paint;

        /// <summary>
        /// Initializes painting view via image.
        /// </summary>
        public PaintingView(Bitmap image)
        {
            Title = "Image editor";

            MinimumSize = new Size(500, paintingPanelWidth);

            Resizable = false;

            this.image = image;

            InitializeComponents();
            Content = GetContent();

            this.Shown += PaintingView_Shown;
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (imagePanel.Uploaded)
            {
                return;
            }

            var dialog = new YesNoDialog()
            {
                Message = "Do you want to close the editor without uploading the picture?",
                Size = new Size(400, 200)
            };

            using (dialog)
            {
                if (dialog.ShowModal() != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void PaintingView_Shown(object sender, EventArgs e)
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
                IsAntialias = true
            };

            imagePanel = new ImagePanel(image);
            imagePanel.ChangePaint(paint);

            paintingPanel = new PaintingPanel();

            paintingPanel.DrawSizeChanged += PaintingPanel_DrawSizeChanged;
            paintingPanel.ColorChanged += PaintingPanel_ColorChanged;
            paintingPanel.StateChanged += PaintingPanel_StateChanged;
            paintingPanel.SaveImageButton.Clicked += SaveImageButton_Clicked;
            paintingPanel.PaintBackButton.Clicked += PaintBackButton_Clicked;
        }

        private void PaintBackButton_Clicked(object sender, EventArgs e)
        {
            imagePanel.PaintBack();
        }

        private void SaveImageButton_Clicked(object sender, EventArgs e)
        {
            var bitmap = imagePanel.ScreenShot();
            ScreenshotManager.UploadScreenShot(bitmap);
            imagePanel.Uploaded = true;
        }

        private void PaintingPanel_StateChanged(object sender, PaintingState state)
        {
            imagePanel.ChangePaintingState(state);
        }

        private void PaintingPanel_ColorChanged(object sender, Color color)
        {
            paint.Color = SkiaSharpHelper.GetSKColorFromEtoColor(color);
            imagePanel.ChangePaint(paint);
        }

        private void PaintingPanel_DrawSizeChanged(object sender, double size)
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
