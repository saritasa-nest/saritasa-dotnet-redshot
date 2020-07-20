using Eto.Forms;
using Eto.Drawing;
using System.IO;
using RedShot.ScreenshotCapture;
using System.Threading.Tasks;
using System.Linq;
using RedShot.Upload;
using System;

namespace RedShot.App
{
    partial class EditorView : Eto.Forms.Form
    {
        int indent = 5;
        ImageView imageview = new ImageView();
        Bitmap sourceImage;
        Point startLocation;
        Point endLocation;
        PixelLayout pixelLayout;
        Form form;
        bool capturing;

        void InitializeComponent()
        {
            this.MouseDown += EditorView_MouseDown;
            this.MouseMove += EditorView_MouseMove;

            var rect = new Rectangle(ScreenShot.GetMainWindowSize());
            var size = new Size(rect.Width, rect.Height);

            ClientSize = size;

            imageview.Image = sourceImage = SetDisplayImage();
            InitHighlightForm();
            pixelLayout = new PixelLayout();
            pixelLayout.Size = new Size(-1, -1);
            pixelLayout.Add(imageview, 0, 0);
            Content = pixelLayout;
            this.Topmost = true;
            this.KeyDown += EditorView_KeyDown;

            Style = "mystyle";

            WindowState = WindowState.Maximized;

            Closing += EditorView_Closing;
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
            form.Topmost = true;
            form.Opacity = 0.5;
            form.ShowInTaskbar = false;
            form.Style = "mystyle";
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
                    location = new Point(endLocation.X + indent, endLocation.Y + indent);
                }
            }

            int formWidth = Math.Abs(endLocation.X - startLocation.X);
            int formHeight = Math.Abs(endLocation.Y - startLocation.Y);

            form.Location = location;
            form.Size = new Size(
                width: formWidth - indent,
                height: formHeight - indent);
        }

        private Bitmap SetDisplayImage()
        {
            return (Bitmap)ScreenShot.TakeScreenshot();
        }
    }
}
