using Eto.Drawing;
using Eto.Forms;
using RedShot.App.Painting.States;
using RedShot.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.App.Painting
{
    internal class PaintingView : Form
    {
        private int paintingPanelWidth = 60;

        private readonly Bitmap image;

        private PaintingPanel paintingPanel;

        private ImagePanel imagePanel;

        private SKPaint paint;

        public PaintingView(Bitmap image)
        {
            MinimumSize = new Size(500, paintingPanelWidth);

            Resizable = false;

            this.image = image;

            InitializeComponents();
            Content = GetContent();
        }

        private void PaintingView_SizeChanged(object sender, EventArgs e)
        {
            paintingPanel.Width = Size.Width;

            imagePanel.Size = new Size(Size.Width, Size.Height - paintingPanelWidth);
        }

        private void InitializeComponents()
        {
            paint = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = 1,
                Style = SKPaintStyle.Stroke,
                StrokeJoin = SKStrokeJoin.Bevel,
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
            ApplicationManager.RunUploadView(bitmap);
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
