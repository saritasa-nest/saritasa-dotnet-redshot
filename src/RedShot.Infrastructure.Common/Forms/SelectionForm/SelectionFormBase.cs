using System;
using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using SkiaSharp;
using RedShot.Resources;

namespace RedShot.Infrastructure.Common.Forms.SelectionForm
{
    /// <summary>
    /// Base form for selection region.
    /// </summary>
    public abstract class SelectionFormBase<T> : Form where T : Form, new()
    {
#region Fields

        /// <summary>
        /// Top message.
        /// </summary>
        protected abstract string TopMessage { get; set; }

        /// <summary>
        /// Disposed flag.
        /// </summary>
        protected bool disposed;

        /// <summary>
        /// Selection manage form.
        /// </summary>
        protected T selectionManageForm;

        /// <summary>
        /// Selection manage form location.
        /// </summary>
        protected Point selectionManageFormLocation;

        /// <summary>
        /// Timer for rendering.
        /// </summary>
        protected UITimer renderTimer;

        /// <summary>
        /// Control for rendering image of the editor.
        /// </summary>
        protected SKControl skcontrol;

        /// <summary>
        /// User's screen snapshot in SkiaSharp format.
        /// </summary>
        protected SKBitmap skScreenImage;

        /// <summary>
        /// User's screen snapshot in ETO format.
        /// </summary>
        protected Bitmap etoScreenImage;

        /// <summary>
        /// Start location of selecting.
        /// </summary>
        protected PointF startLocation;

        /// <summary>
        /// End location of selecting.
        /// </summary>
        protected PointF endLocation;

        /// <summary>
        /// State when user selects region.
        /// </summary>
        protected bool capturing;

        /// <summary>
        /// State when user is selecting region.
        /// </summary>
        protected bool screenSelecting;

        /// <summary>
        /// Return screen which user works with.
        /// </summary>
        private readonly Screen selectionScreen;

        /// <summary>
        /// Selection region size and location.
        /// </summary>
        protected RectangleF selectionRectangle;

        /// <summary>
        /// Size of editor screen.
        /// </summary>
        protected Rectangle screenRectangle;

#endregion Fields

#region Styles
        private DateTime dialogOpenDate;
        private readonly float[] dash = new float[] { 5, 5 };
#endregion Styles

#region Initialization

        /// <summary>
        /// Render frame time in milliseconds.
        /// Should be more than 10 in Linux OS.
        /// </summary>
        private readonly double renderFrameTime = 10;

        protected SelectionFormBase()
        {
            Icon = new Icon(1, Icons.RedCircle);
            selectionScreen = Screen.FromPoint(Mouse.Position);
            InitializeComponents();
            Focus();
        }

        /// <summary>
        /// Initializes whole view.
        /// </summary>
        private void InitializeComponents()
        {
            SetScreenImage();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            dialogOpenDate = DateTime.Now;

            renderTimer = new UITimer();
            renderTimer.Elapsed += RenderFrame;
            renderTimer.Interval = renderFrameTime / 1000;

            skcontrol = new SKControl();
            Content = skcontrol;

            Shown += EditorViewShown;
            UnLoad += EditorViewDrawingSkiaSharpUnLoad;

            InitializeSelectionManageForm();

            SetPlatformOptions();
        }

        private void SetPlatformOptions()
        {
#if _UNIX
            RedShot.Platforms.Linux.GtkHelper.SetFullScreen(this.ControlObject, Size);
#endif
        }

        private void ScreenTimer_Elapsed(object sender, EventArgs e)
        {
            if (screenSelecting)
            {
                var screen = Screen.FromPoint(Mouse.Position);

                if (screen.Bounds != selectionScreen.Bounds)
                {
                    screenSelecting = false;
                    Close();
                    RunNew(this);
                }
            }
        }

        /// <summary>
        /// Runs new selection form.
        /// </summary>
        private static void RunNew(SelectionFormBase<T> form)
        {
            var newForm = (SelectionFormBase<T>)Activator.CreateInstance(form.GetType());
            newForm.Show();
        }

        private void SetScreenImage()
        {
            var rect = new Rectangle(ScreenHelper.GetScreenSize(selectionScreen));
            screenRectangle = rect;
            etoScreenImage = ScreenHelper.TakeScreenshot(selectionScreen);
            skScreenImage = SkiaSharpHelper.ConvertFromEtoBitmap(etoScreenImage);
            Size = new Size(screenRectangle.Width, screenRectangle.Height);
            Location = rect.TopLeft;
        }

        /// <summary>
        /// Renders image for this editor.
        /// </summary>
        private void RenderFrame(object sender, EventArgs e)
        {
            if (!disposed)
            {
                if (capturing)
                {
                    selectionRectangle = EtoDrawingHelper.CreateRectangle(startLocation, endLocation);
                    skcontrol.Execute((surface) => PaintRegion(surface.Canvas));
                }
                else
                {
                    skcontrol.Execute((surface) => PaintClearImage(surface.Canvas));
                }
            }
        }
#endregion Initialization

#region Window events
        private void EditorViewShown(object sender, EventArgs e)
        {
            renderTimer.Start();

            SetSelectionSize();
            MouseUp += EditorViewDrawingSkiaSharpMouseUp;
            MouseDown += EditorViewMouseDown;
            MouseMove += EditorViewMouseMove;
            KeyDown += EditorViewKeyDown;

            skcontrol.Execute((surface) => PaintClearImage(surface.Canvas));
#if _WINDOWS
            StartScreenSelecting();
            BringToFront();
#endif
        }

        private void StartScreenSelecting()
        {
            var screenTimer = new UITimer
            {
                Interval = 0.1
            };
            screenTimer.Elapsed += ScreenTimer_Elapsed;

            screenSelecting = true;
            screenTimer.Start();
        }

        private void SetSelectionSize()
        {
            selectionRectangle = new RectangleF(0, 0, Width, Height);
        }

        private void EditorViewMouseMove(object sender, MouseEventArgs e)
        {
            if (screenSelecting)
            {
                return;
            }
            else if (capturing)
            {
                endLocation = e.Location;
            }
        }

        private void EditorViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (screenSelecting)
                {
                    screenSelecting = false;
                }

                startLocation = e.Location;
                endLocation = e.Location;
                capturing = true;
            }
        }

        private void EditorViewDrawingSkiaSharpMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Alternate)
            {
                Close();
                return;
            }

            if (e.Buttons == MouseButtons.Primary)
            {
                endLocation = e.Location;
                FinishSelection();
            }
        }

        private void EditorViewDrawingSkiaSharpUnLoad(object sender, EventArgs e)
        {
            screenSelecting = false;
            Dispose();
        }

        private void EditorViewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }
#endregion Window events

#region SkiaSharpCommands

        protected virtual void PaintTopMessage(SKCanvas canvas)
        {
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 25
            };

            SKRect textBounds = default;
            textPaint.MeasureText(TopMessage, ref textBounds);

            var xText = Width / 2 - textBounds.MidX;

            canvas.DrawText(TopMessage, xText, 60, textPaint);
        }

        protected virtual void PaintCoordinatePanel(SKCanvas canvas)
        {
            var paint = new SKPaint()
            {
                Color = SKColors.Black,
                TextSize = 14,
                IsAntialias = true
            };

            var text = $"X: {FormatNumber(selectionRectangle.X)} Y: {FormatNumber(selectionRectangle.Y)}" +
                $"   W: {FormatNumber(selectionRectangle.Width)} H: {FormatNumber(selectionRectangle.Height)}";

            var textWidth = paint.MeasureText(text);

            var strokeRectPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White
            };

            var fillRectPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.DeepSkyBlue.WithAlpha(200)
            };

            SKRect textStrokeRect = default;
            SKRect textfillRect = default;
            SKPoint drawCoords;
            if (selectionRectangle.Y > paint.TextSize + 10)
            {
                drawCoords = new SKPoint(selectionRectangle.X + 2, selectionRectangle.Y - 15);
            }
            else if (selectionRectangle.Y + selectionRectangle.Height < Height - paint.TextSize - 10)
            {
                drawCoords = new SKPoint(selectionRectangle.X + 2, selectionRectangle.Y + selectionRectangle.Height + 20);
            }
            else
            {
                drawCoords = new SKPoint(selectionRectangle.X + 6, selectionRectangle.Y + paint.TextSize + 6);
            }

            textStrokeRect.Location = new SKPoint(drawCoords.X - 2, drawCoords.Y - paint.TextSize);
            textStrokeRect.Size = new SKSize(textWidth + 4, paint.TextSize + 4);

            textfillRect.Location = new SKPoint(textStrokeRect.Location.X + 1, textStrokeRect.Location.Y + 1);
            textfillRect.Size = new SKSize(textStrokeRect.Width - 1, textStrokeRect.Height - 1);

            canvas.DrawRect(textfillRect, fillRectPaint);
            canvas.DrawRect(textStrokeRect, strokeRectPaint);

            canvas.DrawText(text, drawCoords, paint);
        }

        protected string FormatNumber(float number)
        {
            return number.ToString("F0");
        }

        protected void PaintBaseImage(SKCanvas canvas)
        {
            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));
        }

        protected virtual void PaintClearImage(SKCanvas canvas)
        {
            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width - 1, Height - 1));

            PaintBaseImage(canvas);
            PaintDarkregion(canvas, editorRect);
            PaintEditorBorder(canvas);
            PaintTopMessage(canvas);
        }

        protected virtual void PaintEditorBorder(SKCanvas canvas)
        {
            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width - 1, Height - 1));
            PaintDashAround(canvas, editorRect, SKColors.Black, SKColors.Red);
        }

        protected virtual void PaintRegion(SKCanvas canvas)
        {
            var size = new SKSize(selectionRectangle.Width, selectionRectangle.Height);
            var point = new SKPoint(selectionRectangle.X, selectionRectangle.Y);

            var regionRect = SKRect.Create(point, size);

            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width, Height));

            PaintBaseImage(canvas);
            PaintDarkregion(canvas, editorRect, regionRect);
            PaintDashAround(canvas, regionRect, SKColors.White, SKColors.Black);
            PaintEditorBorder(canvas);
            PaintCoordinatePanel(canvas);
        }

        protected virtual void PaintDarkregion(SKCanvas canvas, SKRect editorRect, SKRect selectionRect = default)
        {
            var maskPath = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            maskPath.AddRect(editorRect);
            if (selectionRect != default)
            {
                maskPath.AddRect(selectionRect);
            }

            var maskPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(100),
                Style = SKPaintStyle.Fill
            };

            canvas.DrawPath(maskPath, maskPaint);
        }

        protected virtual void PaintDashAround(SKCanvas canvas, SKRect rect, SKColor backColor, SKColor dashColor)
        {
            var rectPaint = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                Color = backColor,
                FilterQuality = SKFilterQuality.High
            };

            // Emulate moving dash
            var movingDashEffect = SKPathEffect.CreateDash(dash, (float)(DateTime.Now - dialogOpenDate).TotalSeconds * -20);
            var rectPaintDash = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                Color = dashColor,
                FilterQuality = SKFilterQuality.High,
                PathEffect = movingDashEffect
            };

            canvas.DrawRect(rect, rectPaint);
            canvas.DrawRect(rect, rectPaintDash);
        }

#endregion SkiaSharpCommands

#region Selection manage form

        protected virtual void InitializeSelectionManageForm()
        {
            selectionManageForm = new T();

            selectionManageFormLocation = new Point(
                (int)(Size.Width - selectionManageForm.Width - Size.Width * 0.05) + Location.X,
                (int)(Size.Height * 0.05) + Location.Y);

            selectionManageForm.Location = selectionManageFormLocation;
            selectionManageForm.KeyDown += EditorViewKeyDown;
            selectionManageForm.Show();
            selectionManageForm.Visible = false;
        }

        #endregion Selection manage form

#region Selection processing

        /// <summary>
        /// Get screen shot.
        /// </summary>
        protected Bitmap GetScreenShot()
        {
            return etoScreenImage.Clone(GetSelectionRegion());
        }

        /// <summary>
        /// Returns rectangle with location regarding screen shot image coordinates.
        /// </summary>
        protected Rectangle GetSelectionRegion()
        {
            if (selectionRectangle.X + selectionRectangle.Width > Width)
            {
                selectionRectangle.Width = Width - selectionRectangle.X;
            }
            if (selectionRectangle.Y + selectionRectangle.Height > Height)
            {
                selectionRectangle.Height = Height - selectionRectangle.Y;
            }

            return (Rectangle)selectionRectangle;
        }

        /// <summary>
        /// Returns rectangle with location regarding selection screen position.
        /// </summary>
        protected Rectangle GetRealSelectionRegion()
        {
            var rect = GetSelectionRegion();

            var realLocation = new Point(Location.X + rect.X, Location.Y + rect.Y);
            rect.Location = realLocation;

            return rect;
        }

        protected abstract void FinishSelection();
        #endregion Selection processing

#region Dispose

        /// <summary>
        /// Disposes UI elements.
        /// </summary>
        public new void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;
                renderTimer?.Dispose();
                etoScreenImage?.Dispose();
                skScreenImage?.Dispose();
                skcontrol?.Dispose();
                selectionManageForm?.Dispose();
            }
        }

#endregion Dispose

    }
}
