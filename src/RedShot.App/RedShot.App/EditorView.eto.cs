using Eto.Forms;
using Eto.Drawing;
using System.IO;
using RedShot.ScreenshotCapture;
using System.Threading.Tasks;
using System.Linq;
using RedShot.Upload;

namespace RedShot.App
{
    partial class EditorView : Eto.Forms.Form
    {
        ImageView imageview = new ImageView();
        Bitmap sourceImage;
        PointF startLocation;
        PointF endLocation;
        PixelLayout pixelLayout;
        Form form;
        bool capturing;

        void InitializeComponent()
        {
            this.MouseDown += EditorView_MouseDown;
            this.MouseMove += EditorView_MouseMove;

            var rect = ScreenshotCapture.ScreenShot.GetMainWindowSize();
            var size = new Size(rect.Width, rect.Height);

            ClientSize = size;

            imageview.Image = sourceImage = SetDisplayImage();
            InitHighlightForm();
            pixelLayout = new PixelLayout();
            pixelLayout.Size = new Size(-1, -1);
            pixelLayout.Add(imageview, 0, 0);
            Content = pixelLayout;

            Style = "mystyle";

            WindowState = WindowState.Maximized;

            Closing += EditorView_Closing;
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
                endLocation = e.Location;
                RenderRectangle();
            }
        }

        private void EditorView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (capturing)
                {
                    endLocation = e.Location;
                    capturing = false;
                    RenderRectangle();
                    form.Visible = false;

                    Upload();
                    Close();
                }
                else
                {
                    startLocation = e.Location;
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
                    location = new Point(startLocation);
                }
                // 3nd state.
                else
                {
                    location = new Point((int)startLocation.X, (int)endLocation.Y);
                }
            }
            else
            {
                // 2st state.
                if (startLocation.Y < endLocation.Y)
                {
                    location = new Point((int)endLocation.X, (int)startLocation.Y);
                }
                // 4nd state.
                else
                {
                    location = new Point(endLocation);
                }
            }

            int formWidth;
            int formHeight;

            if (startLocation.X < endLocation.X)
            {
                formWidth = (int)endLocation.X - (int)startLocation.X;
            }
            else
            {
                formWidth = (int)startLocation.X - (int)endLocation.X;
            }

            if (startLocation.Y < endLocation.Y)
            {
                formHeight = (int)endLocation.Y - (int)startLocation.Y;
            }
            else
            {
                formHeight = (int)startLocation.Y - (int)endLocation.Y;
            }

            form.Location = location;
            form.Size = new Size(
                width: formWidth,
                height: formHeight);
        }

        private Bitmap SetDisplayImage()
        {
            using (var ms = new MemoryStream())
            {
                var bitmap = ScreenShot.TakeScreenshot();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                return new Eto.Drawing.Bitmap(ms);
            }
        }
    }
}
