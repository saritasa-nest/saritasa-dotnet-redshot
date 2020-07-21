using Eto.Forms;
using Eto.Drawing;
using RedShot.ScreenshotCapture;
using RedShot.Upload;
using System;

namespace RedShot.App
{
    internal partial class EditorView : Eto.Forms.Form
    {
        ImageView imageview = new ImageView();
        Bitmap sourceImage;
        Point startLocation;
        Point endLocation;
        PixelLayout pixelLayout;
        Form form;
        bool capturing;

        void InitializeComponent()
        {
            var rect = new Rectangle(ScreenShot.GetMainWindowSize());
            var size = new Size(rect.Width, rect.Height);

            ClientSize = size;

            imageview.Image = sourceImage = SetDisplayImage();
            InitHighlightForm();
            pixelLayout = new PixelLayout();
            pixelLayout.Size = size;
            pixelLayout.Add(imageview, 0, 0);
            Content = pixelLayout;
            Topmost = true;

            Style = "FullScreenStyle";

            WindowState = WindowState.Maximized;

            this.Shown += EditorView_Shown;
        }

        private void EditorView_Shown(object sender, EventArgs e)
        {
            this.MouseDown += EditorView_MouseDown;
            this.MouseMove += EditorView_MouseMove;
            this.KeyDown += EditorView_KeyDown;
            this.Closing += EditorView_Closing;
        }

        private void EditorView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                Close();
            }
        }

        private void EditorView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            form?.Close();
        }

        private void InitHighlightForm()
        {
            form = new Eto.Forms.Form();
            form.BackgroundColor = Colors.Red;
            form.Size = new Size(1, 1);
            form.Topmost = true;
            form.Opacity = 0.4;
            form.ShowInTaskbar = false;
            form.Style = "FullScreenStyle";
            form.Show();

            form.Visible = false;
        }

        private void Upload()
        {
            var rect = new Rectangle(form.Location, form.Size);

            Uploader.UploadImage(sourceImage, rect);
        }

        private void EditorView_MouseMove(object sender, MouseEventArgs e)
        {
            if (capturing)
            {
                endLocation = new Point(e.Location);
                RenderRectangle();
            }
        }

        private void EditorView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (capturing)
                {
                    endLocation = new Point(e.Location);
                    capturing = false;
                    RenderRectangle();
                    form.Visible = false;

                    Upload();
                    Close();
                }
                else
                {
                    startLocation = new Point(e.Location);
                    endLocation = new Point(e.Location);

                    RenderRectangle();

                    capturing = true;
                    form.Visible = true;
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
                    capturing = false;
                    form.Visible = false;
                }
            }
        }

        private void RenderRectangle()
        {
            Point location;

            if (startLocation == endLocation)
            {
                location = startLocation;
            }
            else
            {
                if (startLocation.X < endLocation.X)
                {
                    // 1st state.
                    if (startLocation.Y < endLocation.Y)
                    {
                        location = startLocation;
                    }
                    // 3nd state.
                    else
                    {
                        location = new Point(startLocation.X, endLocation.Y);
                    }
                }
                else
                {
                    // 2st state.
                    if (startLocation.Y < endLocation.Y)
                    {
                        location = new Point(endLocation.X, startLocation.Y);
                    }
                    // 4nd state.
                    else
                    {
                        location = new Point(endLocation.X, endLocation.Y);
                    }
                }
            }

            int formWidth = Math.Abs(endLocation.X - startLocation.X);
            int formHeight = Math.Abs(endLocation.Y - startLocation.Y);

            form.Location = location;
            form.Size = new Size(
                width: formWidth == 0 ? 1 : formWidth,
                height: formHeight == 0 ? 1 : formHeight);
        }

        private Bitmap SetDisplayImage()
        {
            return (Bitmap)ScreenShot.TakeScreenshot();
        }
    }
}
