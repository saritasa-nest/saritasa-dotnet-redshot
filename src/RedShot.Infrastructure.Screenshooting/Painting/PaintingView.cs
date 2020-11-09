using System;
using System.ComponentModel;
using SkiaSharp;
using Eto.Drawing;
using Eto.Forms;
using Prism.Events;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Screenshooting.Painting.States;
using RedShot.Resources;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploaders.Clipboard;
using RedShot.Infrastructure.Uploaders.File;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Abstractions;

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
        private IFile imageFile;

        /// <summary>
        /// Initializes painting view via image.
        /// </summary>
        public PaintingView(Bitmap image)
        {
            Icon = new Icon(1, Icons.RedCircle);
            Title = "Image Editor";
            MinimumSize = new Size(500, paintingPanelWidth);
            Resizable = false;
            this.image = image;
            InitializeComponents();
            Content = GetContent();
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyData == UndoShortcut)
            {
                imagePanel.PaintBack();
            }
            else if (e.KeyData == CopyToClipboardShortcut)
            {
                UploadImageToClipboard();
            }
            e.Handled = true;
        }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

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

        /// <inheritdoc/>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Location = ScreenHelper.GetCenterLocation(Size);
            paintingPanel.BrushEnableButton.Focus();
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

        private void PaintingPanelUploadToFtpSelected(object sender, DataEventArgs<FtpAccount> e)
        {
            var uploadingService = new FtpUploadingService();
            UploadWithBlockingButton(uploadingService.GetFtpUploader(e.Value));
        }

        private void PaintingPanelUploadToFileSelected(object sender, EventArgs e)
        {
            var uploadingService = new FileUploadingService();
            UploadWithBlockingButton(uploadingService.GetUploader());
        }

        private void PaintingPanelUploadToClipboardSelected(object sender, EventArgs e)
        {
            UploadImageToClipboard();
        }

        private void UploadImageToClipboard()
        {
            var uploadingService = new ClipboardUploadingService();
            UploadImage(uploadingService.GetUploader());
        }

        private void UploadWithBlockingButton(IUploader uploader)
        {
            paintingPanel.UploadImageButton.Enabled = false;
            UploadImage(uploader);
            paintingPanel.UploadImageButton.Enabled = true;
        }

        private void UploadImage(IUploader uploader)
        {
            var newImageHash = imagePanel.GetImageHash();
            if (uploadedImageHash != newImageHash)
            {
                uploadedImageHash = newImageHash;
                var file = ScreenshotManager.GetFileFromBitmap(imagePanel.GetPaintingImage());
                imageFile = file;
            }
            UploadingManager.Upload(uploader, imageFile);
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
