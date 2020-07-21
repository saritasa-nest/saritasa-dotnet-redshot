using Eto.Drawing;
using Eto.Forms;
using RedShot.ScreenshotCapture;
using RedShot.Upload;
using System;
using System.Diagnostics;
using System.Threading;

namespace RedShot.App
{
    internal partial class EditorViewDrawing : Eto.Forms.Form
    {
        PixelLayout pixelLayout;
        ImageView imageview = new ImageView();
        Bitmap sourceImage;
        Bitmap image;
        PointF startLocation;
        PointF endLocation;
        bool capturing;
        UITimer timer;
        RectangleF selectionRectangle;
        Rectangle screenRectangle;
        Pen borderDotPen, borderDotPen2;
        Stopwatch penTimer;
        Graphics graphics;

        void InitializeComponent()
        {
            screenRectangle = new Rectangle(ScreenShot.GetMainWindowSize());
            var size = new Size(screenRectangle.Width, screenRectangle.Height);

            ClientSize = size;
            imageview.Image = image = SetDisplayImage();

            sourceImage = image.Clone();

            pixelLayout = new PixelLayout();
            pixelLayout.Size = size;
            pixelLayout.Add(imageview, 0, 0);

            Content = pixelLayout;

            Style = "FullScreenStyle";

            WindowState = WindowState.Maximized;

            borderDotPen = new Pen(Colors.Red, 1);
            borderDotPen2 = new Pen(Colors.White, 1);
            borderDotPen2.DashStyle = new DashStyle(5, 5);
            penTimer = Stopwatch.StartNew();

            graphics = new Graphics(image);
            graphics.ImageInterpolation = ImageInterpolation.None;
            graphics.AntiAlias = false;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;

            timer = new UITimer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 0.001;

            Shown += EditorView_Shown;
        }

        private void EditorView_Shown(object sender, EventArgs e)
        {
            timer.Start();

            this.MouseDown += EditorView_MouseDown;
            this.MouseMove += EditorView_MouseMove;
            this.KeyDown += EditorView_KeyDown;
        }

        private void EditorView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                Close();
            }
        }

        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            if (capturing)
            {
                selectionRectangle = CreateRectangle();
                RenderRectangle();
                Content.Invalidate();
            }
        }

        private void EditorView_MouseMove(object sender, MouseEventArgs e)
        {
            if (capturing)
            {
                endLocation = e.Location;
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

                    Upload();
                    Close();
                }
                else
                {
                    startLocation = e.Location;
                    endLocation = e.Location;
                    capturing = true;
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
                    capturing = false;
                    image = sourceImage.Clone();
                    Content.Invalidate();
                }
                else
                {
                    Close();
                }
            }
        }

        private void RenderRectangle()
        {
            try
            {
                graphics.DrawImage(sourceImage, new PointF(0, 0));

                borderDotPen2.DashStyle = new DashStyle((float)penTimer.Elapsed.TotalSeconds * -15, new float[] { 5, 5 });

                graphics.DrawRectangle(borderDotPen, selectionRectangle);
                graphics.DrawRectangle(borderDotPen2, selectionRectangle);
            }
            catch (OutOfMemoryException)
            {

            }
        }

        private Bitmap SetDisplayImage()
        {
            return (Bitmap)ScreenShot.TakeScreenshot();
        }

        private RectangleF CreateRectangle()
        {
            float width, height;
            float x, y;

            if (startLocation.X <= endLocation.X)
            {
                width = endLocation.X - startLocation.X + 1;
                x = startLocation.X;
            }
            else
            {
                width = startLocation.X - endLocation.X + 1;
                x = endLocation.X;
            }

            if (startLocation.Y <= endLocation.Y)
            {
                height = endLocation.Y - startLocation.Y + 1;
                y = startLocation.Y;
            }
            else
            {
                height = startLocation.Y - endLocation.Y + 1;
                y = endLocation.Y;
            }

            return new RectangleF(x, y, width, height);
        }

        public new void Dispose()
        {
            if (IsDisposed == false)
            {
                graphics?.Dispose();
                borderDotPen?.Dispose();
                borderDotPen2?.Dispose();
                timer?.Dispose();
                base.Dispose();
            }
        }

        private void Upload()
        {
            if (selectionRectangle != default)
            {
                Uploader.UploadImage(sourceImage.Clone((Rectangle)selectionRectangle));
            }
        }
    }
}
