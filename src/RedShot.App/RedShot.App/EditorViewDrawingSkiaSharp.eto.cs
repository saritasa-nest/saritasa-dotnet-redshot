using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.ScreenshotCapture;
using RedShot.Upload;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;

namespace RedShot.App
{
    internal partial class EditorViewDrawingSkiaSharp : Eto.Forms.Form
    {
        bool disposed;
        SKControl skcontrol;
        SKBitmap skScreenImage;
        Bitmap etoScreenImage;
        PointF startLocation;
        PointF endLocation;
        bool capturing;
        UITimer timer;
        RectangleF selectionRectangle;
        Rectangle screenRectangle;
        Stopwatch penTimer;
        float[] dash = new float[] { 5, 5 };

        void InitializeComponent()
        {
            screenRectangle = new Rectangle(ScreenShot.GetMainWindowSize());
            var size = new Size(screenRectangle.Width, screenRectangle.Height);

            ClientSize = size;

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            etoScreenImage = SetDisplayImage();

            skScreenImage = ConvertBitmap(etoScreenImage);

            penTimer = Stopwatch.StartNew();

            timer = new UITimer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 0.01;

            skcontrol = new SKControl();
            Content = skcontrol;

            Shown += EditorView_Shown;
            Closed += EditorViewDrawingSkiaSharp_Closed;
        }

        private void EditorViewDrawingSkiaSharp_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        private void EditorView_Shown(object sender, EventArgs e)
        {
            timer.Start();
            this.MouseDown += EditorView_MouseDown;
            this.MouseMove += EditorView_MouseMove;
            this.KeyDown += EditorView_KeyDown;

            skcontrol.Execute((surface) => PaintClearImage(surface));
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
            if (capturing && disposed == false)
            {
                selectionRectangle = CreateRectangle();
                RenderRectangle();
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
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                }
                else
                {
                    Close();
                }
            }
        }

        private void RenderRectangle()
        {
            skcontrol.Execute((surface) => PaintRegion(surface));
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
            if (disposed == false)
            {
                disposed = true;
                skcontrol?.Dispose();
                timer?.Dispose();
                skScreenImage?.Dispose();
                etoScreenImage?.Dispose();

                if (IsDisposed == false)
                {
                    base.Dispose();
                }
            }
        }

        private void Upload()
        {
            if (selectionRectangle != default)
            {
                Uploader.UploadImage(etoScreenImage.Clone((Rectangle)selectionRectangle));
            }
        }

        private void PaintClearImage(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));
        }

        private void PaintRegion(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));

            var rectPaint = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                FilterQuality = SKFilterQuality.Low
            };

            var rectPaintDash = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                FilterQuality = SKFilterQuality.Low,
                PathEffect = SKPathEffect.CreateDash(dash, (float)penTimer.Elapsed.TotalSeconds * -15)
            };

            var size = new SKSize(selectionRectangle.Width, selectionRectangle.Height);
            var point = new SKPoint(selectionRectangle.X, selectionRectangle.Y);

            var rect = SKRect.Create(point, size);

            canvas.DrawRect(rect, rectPaint);
            canvas.DrawRect(rect, rectPaintDash);
        }

        private SKBitmap ConvertBitmap(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bitmap);
                ms.Seek(0, SeekOrigin.Begin);

                return SKBitmap.Decode(ms);
            }
        }
    }
}
