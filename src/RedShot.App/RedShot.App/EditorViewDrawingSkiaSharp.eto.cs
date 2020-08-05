using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using SkiaSharp;
using RedShot.Helpers.EditorView;
using RedShot.Helpers;

namespace RedShot.App
{
    /// <summary>
    /// Shapshot editor view.
    /// Functions: Select, Move, Resize, Paint.
    /// </summary>
    internal partial class EditorViewDrawingSkiaSharp : Form
    {
        private bool disposed;

        private ScreenShotPanel screenShotPanel;
        private Point screenShotPanelLocation;

        /// <summary>
        /// Render frametime in milliseconds.
        /// Should be more than 10 in Linux OS.
        /// </summary>
        private const double renderFrameTime = 10;

        /// <summary>
        /// Timer for rendering.
        /// </summary>
        private UITimer timer;

        /// <summary>
        /// Control for rendering image of the editor.
        /// </summary>
        private SKControl skcontrol;

        /// <summary>
        /// User's screen snapshot in SkiaSharp format.
        /// </summary>
        private SKBitmap skScreenImage;

        /// <summary>
        /// User's screen snapshot in Eto format.
        /// </summary>
        private Bitmap etoScreenImage;

        /// <summary>
        /// Start location of selecting.
        /// </summary>
        private PointF startLocation;

        /// <summary>
        /// End location of selecting.
        /// </summary>
        private PointF endLocation;

        /// <summary>
        /// State when user selects region.
        /// </summary>
        private bool capturing;

        /// <summary>
        /// State when user has selected region.
        /// </summary>
        private bool captured;

        /// <summary>
        /// Selection region size and location.
        /// </summary>
        private RectangleF selectionRectangle;

        /// <summary>
        /// Size of editor screen.
        /// </summary>
        private Rectangle screenRectangle;

        /// <summary>
        /// For beauty.
        /// </summary>
        #region Styles
        private Stopwatch penTimer;
        private float[] dash = new float[] { 5, 5 };
        #endregion Styles

        #region Movingfields
        private bool moving;
        private float relativeX;
        private float relativeY;
        #endregion Movingfields

        /// <summary>
        /// Fileds for resizing selected area.
        /// </summary>
        #region ResizingFields
        private bool resizing;
        private ResizePart resizePart;
        private LineF oppositeBorder;
        private PointF oppositeAngle;
        #endregion Resizingfields

        /// <summary>
        /// Initializes whole view.
        /// </summary>
        void InitializeComponent()
        {
            screenRectangle = new Rectangle(ScreenHelper.GetMainWindowSize());
            var size = new Size(screenRectangle.Width, screenRectangle.Height);
            Size = size;

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            etoScreenImage = ScreenHelper.TakeScreenshot();
            skScreenImage = SkiaSharpHelper.ConvertFromEtoBitmap(etoScreenImage);

            penTimer = Stopwatch.StartNew();

            timer = new UITimer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = renderFrameTime / 1000;

            skcontrol = new SKControl();
            Content = skcontrol;

            Shown += EditorView_Shown;
            UnLoad += EditorViewDrawingSkiaSharp_UnLoad;

            InitializeScreenShotPanel();
        }

        /// <summary>
        /// Renders image for this editor.
        /// </summary>
        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            if (disposed == false)
            {
                if (capturing)
                {
                    selectionRectangle = EtoDrawingHelper.CreateRectangle(startLocation, endLocation);
                    skcontrol.Execute((surface) => PaintRegion(surface));
                }
                else if (captured)
                {
                    skcontrol.Execute((surface) => PaintRegion(surface));
                }
                else
                {
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                }
            }
        }

        /// <summary>
        /// For changing mouse pointer.
        /// </summary>
        #region PointerFunctions

        private void SetDefaultPointer()
        {
            Cursor = Cursors.Arrow;
        }

        private void SetMousePointer(PointF location)
        {
            if (CheckOnResizing(location))
            {
                if (resizePart == ResizePart.Angle)
                {
                    Cursor = Cursors.Crosshair;
                }
                else if (resizePart == ResizePart.HorizontalBorder)
                {
                    Cursor = Cursors.HorizontalSplit;
                }
                else if (resizePart == ResizePart.VerticalBorder)
                {
                    Cursor = Cursors.VerticalSplit;
                }
            }
            else if (CheckOnMoving(location))
            {
                Cursor = Cursors.Move;
            }
            else
            {
                SetDefaultPointer();
            }
        }

        #endregion PointerFunctions

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

            skcontrol.Execute((surface) => PaintClearImage(surface));
        }

        private void EditorView_MouseMove(object sender, MouseEventArgs e)
        {
            if (capturing)
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
                if (capturing)
                {
                    endLocation = e.Location;
                    capturing = false;

                    captured = true;
                    ShowPointPaintingView();
                }
                else if (captured)
                {
                    if (CheckOnResizing(e.Location))
                    {
                        resizing = true;
                    }
                    else if (CheckOnMoving(e.Location))
                    {
                        moving = true;
                    }
                    HidePointPaintingView();
                }
                else
                {
                    startLocation = e.Location;
                    endLocation = e.Location;
                    capturing = true;
                    HidePointPaintingView();
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
                    capturing = false;
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                }
                else if (captured)
                {
                    captured = false;
                    moving = false;
                    resizing = false;
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                    HidePointPaintingView();
                    SetDefaultPointer();
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
                    ShowPointPaintingView();
                }
            }
        }

        private void EditorViewDrawingSkiaSharp_UnLoad(object sender, EventArgs e)
        {
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
                    SaveScreenShot();
                    break;
            }
        }
        #endregion WindowEvents

        /// <summary>
        /// Skia sharp drawing functions.
        /// </summary>
        #region SkiaSharpCommands

        private void PaintTopMessage(SKSurface surface)
        {
            string message = "Please select a region to capture";

            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 25
            };

            SKRect textBounds = default;
            textPaint.MeasureText(message, ref textBounds);

            var xText = Width / 2 - textBounds.MidX;

            surface.Canvas.DrawText(message, xText, 60, textPaint);
        }

        private void PaintCoordinatePanel(SKSurface surface)
        {
            var canvas = surface.Canvas;

            var paint = new SKPaint()
            {
                Color = SKColors.Black,
                TextSize = 14,
                IsAntialias = true
            };

            var text = $"X: {(int)selectionRectangle.X} Y: {(int)selectionRectangle.Y}   W: {(int)selectionRectangle.Width} H: {(int)selectionRectangle.Height}";

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

            surface.Canvas.DrawText(text, drawCoords, paint);
        }

        private void PaintClearImage(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));

            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width - 1, Height - 1));

            PaintDarkregion(surface, editorRect);

            PaintEditorBorder(surface);

            PaintTopMessage(surface);
        }

        private void PaintEditorBorder(SKSurface surface)
        {
            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width - 1, Height - 1));
            PaintDashAround(surface, editorRect, SKColors.Black, SKColors.Red);
        }

        private void PaintRegion(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));

            var size = new SKSize(selectionRectangle.Width, selectionRectangle.Height);
            var point = new SKPoint(selectionRectangle.X, selectionRectangle.Y);

            var regionRect = SKRect.Create(point, size);

            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width, Height));

            PaintDarkregion(surface, editorRect, regionRect);

            PaintDashAround(surface, regionRect, SKColors.White, SKColors.Black);

            PaintEditorBorder(surface);

            PaintCoordinatePanel(surface);
        }

        private void PaintDarkregion(SKSurface surface, SKRect editorRect, SKRect selectionRect = default)
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
            surface.Canvas.DrawPath(maskPath, maskPaint);
        }

        private void PaintDashAround(SKSurface surface, SKRect rect, SKColor backColor, SKColor dashColor)
        {
            var canvas = surface.Canvas;

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

        #region ScreenShotPanel

        private void InitializeScreenShotPanel()
        {
            screenShotPanel = new ScreenShotPanel();

            screenShotPanel.EnablePaintingModeButton.Clicked += PointPaintingPanel_PaintingModeEnabled;
            screenShotPanel.SaveScreenShotButton.Clicked += SaveScreenShotButton_Clicked;


            screenShotPanelLocation = new Point(
                (int)(Size.Width - screenShotPanel.Width - Size.Width * 0.05),
                (int)(Size.Height * 0.05));

            screenShotPanel.Location = screenShotPanelLocation;

            screenShotPanel.KeyDown += EditorView_KeyDown;

            screenShotPanel.Show();
            screenShotPanel.Visible = false;
        }

        private void SaveScreenShotButton_Clicked(object sender, EventArgs e)
        {
            SaveScreenShot();
        }

        private void ShowPointPaintingView()
        {
            screenShotPanel.Visible = true;
            screenShotPanel.Location = screenShotPanelLocation;
        }

        private void HidePointPaintingView()
        {
            screenShotPanel.Visible = false;
        }

        private void PointPaintingPanel_PaintingModeEnabled(object sender, EventArgs e)
        {
            var screenshot = GetScreenShot();
            ApplicationManager.RunPaintingView(screenshot);
        }

        #endregion ScreenShotPanel

        /// <summary>
        /// Runs uploader functions.
        /// </summary>
        #region Uploading
        private Bitmap GetScreenShot()
        {
            if (selectionRectangle != default)
            {
                if (selectionRectangle.X + selectionRectangle.Width > Width)
                {
                    selectionRectangle.Width = Width - selectionRectangle.X;
                }
                if (selectionRectangle.Y + selectionRectangle.Height > Height)
                {
                    selectionRectangle.Height = Height - selectionRectangle.Y;
                }

                return etoScreenImage.Clone((Rectangle)selectionRectangle);
            }

            return null;
        }

        #endregion Uploading

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

        private void SaveScreenShot()
        {
            if (captured)
            {
                ApplicationManager.RunUploadView(GetScreenShot());
                Close();
            }
        }

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
                screenShotPanel?.Dispose();
            }
        }
    }
}
