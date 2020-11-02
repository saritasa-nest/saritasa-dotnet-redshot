using System;
using System.ComponentModel;
using SkiaSharp;
using Eto.Drawing;
using Eto.Forms;
using Prism.Events;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Screenshooting.Painting.States;
using RedShot.Resources;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploaders.Clipboard;
using RedShot.Infrastructure.Uploaders.File;

namespace RedShot.Infrastructure.Screenshooting.Painting
{
    /// <summary>
    /// Painting view.
    /// </summary>
    internal class PaintingView : Form
    {
        private const Keys UndoShortcut = Keys.Control | Keys.Z;
        private const Keys CopyToClipboardShortcut = Keys.Control | Keys.C;

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
            this.KeyUp += PaintingViewKeyUp;
        }

        private void PaintingViewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == UndoShortcut)
            {
                imagePanel.PaintBack();
            }
            else if (e.KeyData == CopyToClipboardShortcut)
            {
                UploadImageToClipboard();
            }
        }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            imagePanel.TextInputView?.Close();

            if (uploadedImageHash == imagePanel.GetImageHash())
            {
                return;
            }

            const string message = "Do you want to close the editor without uploading the image?";
            const string title = "RedShot Question";
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
            imagePanel = new ImagePanel(image);

            paintingPanel = new PaintingPanel();
            paintingPanel.DrawSizeChanged += PaintingPanelDrawSizeChanged;
            paintingPanel.ColorChanged += PaintingPanelColorChanged;
            paintingPanel.StateChanged += PaintingPanelStateChanged;
            paintingPanel.PaintBackButton.Clicked += PaintBackButtonClicked;

            paintingPanel.UploadImageButton.UploadToClipboardSelected += PaintingPanelUploadToClipboardSelected;
            paintingPanel.UploadImageButton.UploadToFileSelected += PaintingPanelUploadToFileSelected;
            paintingPanel.UploadImageButton.UploadToFtpSelected += PaintingPanelUploadToFtpSelected;

            SetDefaultOptions(imagePanel);
        }

        private void PaintingPanelUploadToFtpSelected(object sender, DataEventArgs<FtpAccount> e)
        {
            var uploader = new FtpUploadingService();
            var file = ScreenshotManager.GetFileFromBitmap(imagePanel.GetPaintingImage());

            UploadingManager.Upload(uploader.GetUploader(e.Value), file);
        }

        private void PaintingPanelUploadToFileSelected(object sender, EventArgs e)
        {
            var uploader = new FileUploadingService();
            var file = ScreenshotManager.GetFileFromBitmap(imagePanel.GetPaintingImage());

            UploadingManager.Upload(uploader.GetUploader(), file);
        }

        private void PaintingPanelUploadToClipboardSelected(object sender, EventArgs e)
        {
            UploadImageToClipboard();
        }

        private void UploadImageToClipboard()
        {
            var uploader = new ClipboardUploadingService();
            var file = ScreenshotManager.GetFileFromBitmap(imagePanel.GetPaintingImage());

            UploadingManager.Upload(uploader.GetUploader(), file);
        }

        private void SetDefaultOptions(ImagePanel imagePanel)
        {
            paint = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = 3,
                Style = SKPaintStyle.Stroke,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            imagePanel.ChangePaint(paint);
            imagePanel.ChangePaintingState(PaintingState.Brush);
        }

        private void PaintBackButtonClicked(object sender, EventArgs e)
        {
            imagePanel.PaintBack();
        }

        private void SaveImageButtonClicked(object sender, EventArgs e)
        {
            var bitmap = imagePanel.GetPaintingImage();
            ScreenshotManager.UploadScreenShot(bitmap);
            uploadedImageHash = imagePanel.GetImageHash();
        }

        private void PaintingPanelStateChanged(object sender, DataEventArgs<PaintingState> e)
        {
            imagePanel.ChangePaintingState(e.Value);
        }

        private void PaintingPanelColorChanged(object sender, DataEventArgs<Color> e)
        {
            paint.Color = SkiaSharpHelper.GetSKColorFromEtoColor(e.Value);
            imagePanel.ChangePaint(paint);
        }

        private void PaintingPanelDrawSizeChanged(object sender, DataEventArgs<float> e)
        {
            paint.StrokeWidth = e.Value;
            paint.TextSize = e.Value;

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
