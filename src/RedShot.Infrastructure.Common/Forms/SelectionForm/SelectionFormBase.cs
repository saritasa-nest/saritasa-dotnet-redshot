using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using SkiaSharp;

namespace RedShot.Infrastructure.Common.Forms.SelectionForm
{
    /// <summary>
    /// Base form for selection region.
    /// </summary>
    public abstract class SelectionFormBase<T> : Form where T : Form, new()
    {
        /// <summary>
        /// Top message.
        /// </summary>
        protected virtual string TopMessage { get; set; } = "Please select a region";

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
        protected UITimer timer;

        /// <summary>
        /// Control for rendering image of the editor.
        /// </summary>
        protected SKControl skcontrol;

        /// <summary>
        /// User's screen snapshot in SkiaSharp format.
        /// </summary>
        protected SKBitmap skScreenImage;

        /// <summary>
        /// User's screen snapshot in Eto format.
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
        /// State when user has selected region.
        /// </summary>
        protected bool captured;

        /// <summary>
        /// State when user is selecting region.
        /// </summary>
        protected bool screenSelecting;

        /// <summary>
        /// Return screen which user works with.
        /// </summary>
        public static Screen selectionScreen;

        /// <summary>
        /// Selection region size and location.
        /// </summary>
        protected RectangleF selectionRectangle;

        /// <summary>
        /// Size of editor screen.
        /// </summary>
        protected Rectangle screenRectangle;

        /// <summary>
        /// For beauty.
        /// </summary>
        #region Styles
        private Cursor capturedCursor;
        private Stopwatch penTimer;
        private float[] dash = new float[] { 5, 5 };
        #endregion Styles

        #region Movingfields
        private bool moving;
        private float relativeX;
        private float relativeY;
        #endregion Movingfields

        /// <summary>
        /// Fields for resizing selected area.
        /// </summary>
        #region ResizingFields
        private bool resizing;
        private ResizePart resizePart;
        private LineF oppositeBorder;
        private PointF oppositeAngle;
        #endregion Resizingfields

        /// <summary>
        /// Render frametime in milliseconds.
        /// Should be more than 10 in Linux OS.
        /// </summary>
        private readonly double renderFrameTime = 10;

        protected SelectionFormBase()
        {
            selectionScreen = Screen.FromPoint(Mouse.Position);
            InitializeComponent();
        }

        UITimer screenTimer;

        /// <summary>
        /// Initializes whole view.
        /// </summary>
        private void InitializeComponent()
        {
            SetScreenImage();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            SetPlatformOptions();

            penTimer = Stopwatch.StartNew();

            timer = new UITimer();
            timer.Elapsed += RenderFrame;
            timer.Interval = renderFrameTime / 1000;

            skcontrol = new SKControl();
            Content = skcontrol;

            Shown += EditorView_Shown;
            UnLoad += EditorViewDrawingSkiaSharp_UnLoad;

            InitializeSelectionManageForm();

            screenTimer = new UITimer();
            screenTimer.Interval = 0.1;
            screenTimer.Elapsed += ScreenTimer_Elapsed;

            var capturedIcon = new Bitmap(Resources.Properties.Resources.Pointer);
            capturedCursor = FormsHelper.GetCursor(capturedIcon, new Size(20, 20), new Point(3, 3));
        }

        private void SetPlatformOptions()
        {
#if _WINDOWS
            Topmost = true;
#elif _UNIX
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
                    selectionScreen = screen;
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
                else if (captured)
                {
                    skcontrol.Execute((surface) => PaintRegion(surface.Canvas));
                }
                else
                {
                    skcontrol.Execute((surface) => PaintClearImage(surface.Canvas));
                }
            }
        }

        /// <summary>
        /// For changing mouse pointer.
        /// </summary>
#region PointerFunctions

        private void SetMousePointer(PointF location)
        {
            if (CheckOnResizing(location))
            {
                switch (resizePart)
                {
                    case ResizePart.Angle:
                        Cursor = Cursors.Crosshair;
                        break;

                    case ResizePart.HorizontalBorder:
                        Cursor = Cursors.HorizontalSplit;
                        break;

                    case ResizePart.VerticalBorder:
                        Cursor = Cursors.VerticalSplit;
                        break;
                }
            }
            else if (CheckOnMoving(location))
            {
                Cursor = Cursors.Move;
            }
            else if (captured)
            {
                Cursor = capturedCursor;
            }
            else
            {
                SetDefaultPointer();
            }
        }

        private void SetDefaultPointer()
        {
            Cursor = Cursors.Arrow;
        }

#endregion PointerFunctions

        /// <summary>
        /// Handlers for window events.
        /// </summary>
#region WindowEvents
        private void EditorView_Shown(object sender, EventArgs e)
        {
            timer.Start();
            MouseUp += EditorViewDrawingSkiaSharp_MouseUp;
            MouseDown += EditorView_MouseDown;
            MouseMove += EditorView_MouseMove;
            KeyDown += EditorView_KeyDown;

            skcontrol.Execute((surface) => PaintClearImage(surface.Canvas));
            screenSelecting = true;
            screenTimer.Start();
        }

        private void EditorView_MouseMove(object sender, MouseEventArgs e)
        {
            if (screenSelecting)
            {
                return;
            }
            else if (capturing)
            {
                endLocation = e.Location;
            }
            else if (captured)
            {
                if (moving)
                {
                    MoveSelectionArea(e.Location);
                }
                else if (resizing)
                {
                    ResizeSelectionArea(e.Location);
                }
                else
                {
                    SetMousePointer(e.Location);
                }
            }
        }

        private void EditorView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (screenSelecting)
                {
                    screenSelecting = false;
                }

                if (capturing)
                {
                    endLocation = e.Location;
                    capturing = false;

                    captured = true;
                    ShowSelectionManageForm();
                    return;
                }

                if (captured)
                {
                    if (CheckOnResizing(e.Location))
                    {
                        resizing = true;
                    }
                    else if (CheckOnMoving(e.Location))
                    {
                        moving = true;
                    }
                }
                else
                {
                    startLocation = e.Location;
                    endLocation = e.Location;
                    capturing = true;
                }

                HideSelectionManageForm();
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
                    capturing = false;
                    screenSelecting = true;
                }
                else if (captured)
                {
                    captured = false;
                    moving = false;
                    resizing = false;
                    HideSelectionManageForm();
                    SetDefaultPointer();
                    screenSelecting = true;
                }
                else
                {
                    Close();
                }
            }
        }

        private void EditorViewDrawingSkiaSharp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                moving = false;
                resizing = false;
                SetDefaultPointer();

                if (captured)
                {
                    ShowSelectionManageForm();
                }
            }
        }

        private void EditorViewDrawingSkiaSharp_UnLoad(object sender, EventArgs e)
        {
            screenSelecting = false;
            Dispose();
        }

        private void EditorView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    Close();
                    break;

                case Keys.Enter:
                    FinishSelection();
                    break;
            }
        }
#endregion WindowEvents

        /// <summary>
        /// Skia sharp drawing functions.
        /// </summary>
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

            SKPoint drawCoords = default;
            SKRect textStrokeRect = default;
            SKRect textfillRect = default;

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
            // the path to use as a mask
            var maskPath = new SKPath();
            maskPath.FillType = SKPathFillType.EvenOdd; // make the first rect dark, cut the next
            maskPath.AddRect(editorRect);
            if (selectionRect != default)
            {
                maskPath.AddRect(selectionRect);
            }

            // the dark paint overlay
            var maskPaint = new SKPaint();
            maskPaint.Color = SKColors.Black.WithAlpha(100);
            maskPaint.Style = SKPaintStyle.Fill;

            // draw
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

            var rectPaintDash = new SKPaint
            {
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                Color = dashColor,
                FilterQuality = SKFilterQuality.High,
                PathEffect = SKPathEffect.CreateDash(dash, (float)penTimer.Elapsed.TotalSeconds * -20)
            };

            canvas.DrawRect(rect, rectPaint);
            canvas.DrawRect(rect, rectPaintDash);
        }

#endregion SkiaSharpCommands

#region MovingResizing

#region Checking

        /// <summary>
        /// Checks whether user can move selected area.
        /// </summary>
        private bool CheckOnMoving(PointF mouseLocation)
        {
            if (mouseLocation.X >= selectionRectangle.X && mouseLocation.X <= selectionRectangle.X + selectionRectangle.Width)
            {
                if (mouseLocation.Y >= selectionRectangle.Y && mouseLocation.Y <= selectionRectangle.Y + selectionRectangle.Height)
                {
                    relativeX = mouseLocation.X - selectionRectangle.X;
                    relativeY = mouseLocation.Y - selectionRectangle.Y;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether user can resize selected area.
        /// </summary>
        private bool CheckOnResizing(PointF mouseLocation)
        {
            if (ResizeHelper.ApproximatelyEquals(selectionRectangle.Y, mouseLocation.Y))
            {
                if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y + selectionRectangle.Height);
                }
                else if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X + selectionRectangle.Width, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(selectionRectangle.X, selectionRectangle.Y + selectionRectangle.Height);
                }
                else if (mouseLocation.X > selectionRectangle.X && mouseLocation.X < selectionRectangle.X + selectionRectangle.Width)
                {
                    resizePart = ResizePart.HorizontalBorder;

                    var start = new PointF(selectionRectangle.X, selectionRectangle.Y + selectionRectangle.Height);
                    var end = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y + selectionRectangle.Height);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(selectionRectangle.Y + selectionRectangle.Height, mouseLocation.Y))
            {
                if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y);
                }
                else if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X + selectionRectangle.Width, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(selectionRectangle.X, selectionRectangle.Y);
                }
                else if (mouseLocation.X > selectionRectangle.X && mouseLocation.X < selectionRectangle.X + selectionRectangle.Width)
                {
                    resizePart = ResizePart.HorizontalBorder;

                    var start = new PointF(selectionRectangle.X, selectionRectangle.Y);
                    var end = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X, mouseLocation.X))
            {
                if (mouseLocation.Y > selectionRectangle.Y && mouseLocation.Y < selectionRectangle.Y + Height)
                {
                    resizePart = ResizePart.VerticalBorder;

                    var start = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y);
                    var end = new PointF(selectionRectangle.X + selectionRectangle.Width, selectionRectangle.Y + selectionRectangle.Height);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(selectionRectangle.X + selectionRectangle.Width, mouseLocation.X))
            {
                if (mouseLocation.Y > selectionRectangle.Y && mouseLocation.Y < selectionRectangle.Y + Height)
                {
                    resizePart = ResizePart.VerticalBorder;

                    var start = new PointF(selectionRectangle.X, selectionRectangle.Y);
                    var end = new PointF(selectionRectangle.X, selectionRectangle.Y + selectionRectangle.Height);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

#endregion Checking

        /// <summary>
        /// Moves selection area.
        /// </summary>
        private void MoveSelectionArea(PointF location)
        {
            var newXcoord = location.X - relativeX;
            var newYcoord = location.Y - relativeY;

            if (newXcoord >= 0 && newYcoord >= 0)
            {
                if (newXcoord + selectionRectangle.Width < Size.Width)
                {
                    selectionRectangle.X = newXcoord;
                }
                else
                {
                    selectionRectangle.X = Size.Width - selectionRectangle.Width;
                }

                if (newYcoord + selectionRectangle.Height <= Size.Height)
                {
                    selectionRectangle.Y = newYcoord;
                }
                else
                {
                    selectionRectangle.Y = Size.Height - selectionRectangle.Height;
                }
            }
        }

        /// <summary>
        /// Resizes selection area.
        /// </summary>
        private void ResizeSelectionArea(PointF location)
        {
            if (resizePart == ResizePart.Angle)
            {
                selectionRectangle = EtoDrawingHelper.CreateRectangle(location, oppositeAngle);
            }
            else if (resizePart == ResizePart.HorizontalBorder)
            {
                var point = new PointF(oppositeBorder.StartPoint.X, location.Y);
                selectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
            }
            else if (resizePart == ResizePart.VerticalBorder)
            {
                var point = new PointF(location.X, oppositeBorder.StartPoint.Y);
                selectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
            }
        }

#endregion MovingResizing

#region SelectionManageForm

        protected virtual void InitializeSelectionManageForm()
        {
            selectionManageForm = new T();

            selectionManageFormLocation = new Point(
                (int)(Size.Width - selectionManageForm.Width - Size.Width * 0.05) + Location.X,
                (int)(Size.Height * 0.05) + Location.Y);

            selectionManageForm.Location = selectionManageFormLocation;

            selectionManageForm.KeyDown += EditorView_KeyDown;

            selectionManageForm.Show();
            selectionManageForm.Visible = false;
        }

        private void ShowSelectionManageForm()
        {
            selectionManageForm.Visible = true;
            selectionManageForm.Location = selectionManageFormLocation;
        }

        private void HideSelectionManageForm()
        {
            selectionManageForm.Visible = false;
        }

#endregion SelectionManageForm

        /// <summary>
        /// Returns rectangle with location regarding screenshot image coordinates.
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

        /// <summary>
        /// Disposes UI elements.
        /// </summary>
        public new void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;
                timer?.Dispose();
                etoScreenImage?.Dispose();
                skScreenImage?.Dispose();
                skcontrol?.Dispose();
                selectionManageForm?.Dispose();
            }
        }
    }
}
