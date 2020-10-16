﻿using System;
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
        private bool uploaded;

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

            this.Shown += PaintingView_Shown;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            imagePanel.TextInputView?.Close();

            if (uploaded)
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
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            imagePanel = new ImagePanel(image);
            imagePanel.ChangePaint(paint);
            imagePanel.ImageChanged += ImagePanelImageChanged;

            paintingPanel = new PaintingPanel();

            paintingPanel.DrawSizeChanged += PaintingPanel_DrawSizeChanged;
            paintingPanel.ColorChanged += PaintingPanel_ColorChanged;
            paintingPanel.StateChanged += PaintingPanel_StateChanged;
            paintingPanel.SaveImageButton.Clicked += SaveImageButton_Clicked;
            paintingPanel.PaintBackButton.Clicked += PaintBackButton_Clicked;
        }

        private void ImagePanelImageChanged(object sender, EventArgs e)
        {
            uploaded = false;
        }

        private void PaintBackButton_Clicked(object sender, EventArgs e)
        {
            imagePanel.PaintBack();
        }

        private void SaveImageButton_Clicked(object sender, EventArgs e)
        {
            var bitmap = imagePanel.ScreenShot();
            ScreenshotManager.UploadScreenShot(bitmap);
            uploaded = true;
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
