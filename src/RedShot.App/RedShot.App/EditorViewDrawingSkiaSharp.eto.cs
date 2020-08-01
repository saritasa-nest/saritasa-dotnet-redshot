using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.App.Helpers;
using RedShot.Helpers;
using RedShot.Upload;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RedShot.App
{
    internal partial class EditorViewDrawingSkiaSharp : Eto.Forms.Form
    {
        // Milliseconds.
        double renderFrameTime = 10;
        bool disposed;
        SKControl skcontrol;
        SKBitmap skScreenImage;
        Bitmap etoScreenImage;
        PointF startLocation;
        PointF endLocation;
        bool capturing;
        bool captured;
        UITimer timer;
        RectangleF selectionRectangle;
        Rectangle screenRectangle;
        Stopwatch penTimer;
        float[] dash = new float[] { 5, 5 };

        #region PointPainting

        List<HashSet<PointF>> pointPolygons = new List<HashSet<PointF>>();

        HashSet<PointF> currentPointPolygons = new HashSet<PointF>();

        bool paintPointsRequested;

        bool pointsPainting;

        PointPaintingView pointPaintingPanel;

        #endregion PointPainting

        #region Movingfields
        bool moving;
        float relativeX;
        float relativeY;
        #endregion Movingfields

        #region ResizingFields
        bool resizing;
        ResizePart resizePart;
        LineF oppositeBorder;
        PointF oppositeAngle;
        #endregion Resizingfields

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

            SetDefaultPointer();
            InitializePainting();
        }

        /// <summary>
        /// Render image for this editor.
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

        #region PointerFunctions

        private void SetDefaultPointer()
        {
            Cursor = Cursors.Arrow;
        }

        private void SetMousePointer(PointF location)
        {
            if (paintPointsRequested)
            {
                // Set some drawing pointer :) .
            }
            else if (CheckOnResizing(location))
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
                    var newXcoord = e.Location.X - relativeX;
                    var newYcoord = e.Location.Y - relativeY;

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
                else if (resizing)
                {
                    if (resizePart == ResizePart.Angle)
                    {
                        selectionRectangle = EtoDrawingHelper.CreateRectangle(e.Location, oppositeAngle);
                    }
                    else if (resizePart == ResizePart.HorizontalBorder)
                    {
                        var point = new PointF(oppositeBorder.StartPoint.X, e.Location.Y);
                        selectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
                    }
                    else if (resizePart == ResizePart.VerticalBorder)
                    {
                        var point = new PointF(e.Location.X, oppositeBorder.StartPoint.Y);
                        selectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
                    }
                }
                else if (pointsPainting)
                {
                    currentPointPolygons.Add(e.Location);
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
                    if (paintPointsRequested)
                    {
                        pointsPainting = true;
                        currentPointPolygons = new HashSet<PointF>();
                        pointPolygons.Add(currentPointPolygons);
                    }
                    else
                    {
                        if (CheckOnResizing(e.Location))
                        {
                            resizing = true;
                            HidePointPaintingView();
                        }
                        else if (CheckOnMoving(e.Location))
                        {
                            moving = true;
                            HidePointPaintingView();
                        }
                    }
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
                    ShowPointPaintingView();
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                }
                else if (captured)
                {
                    captured = false;
                    moving = false;
                    resizing = false;
                    skcontrol.Execute((surface) => PaintClearImage(surface));
                    HidePointPaintingView();
                    pointPolygons.Clear();
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
                pointsPainting = false;
                SetDefaultPointer();

                if (captured && paintPointsRequested == false)
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
                    if (captured)
                    {
                        Upload();
                        Close();
                    }
                    break;
            }
        }
        #endregion WindowEvents

        #region SkiaSharpCommands

        private void PaintPoints(SKSurface surface)
        {
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = 4,
                StrokeJoin = SKStrokeJoin.Bevel,
                IsAntialias = true
            };

            foreach (var points in pointPolygons)
            {
                surface.Canvas.DrawPoints(SKPointMode.Polygon, points.Select(p => new SKPoint(p.X, p.Y)).ToArray(), paint);
            }
        }

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

            var text = $"X: {selectionRectangle.X} Y: {selectionRectangle.Y}   W: {selectionRectangle.Width} H: {selectionRectangle.Height}";

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

            PaintPoints(surface);

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

            PaintPoints(surface);

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

        #region Painting

        private void InitializePainting()
        {
            pointPaintingPanel = new PointPaintingView();

            pointPaintingPanel.ClearButton.Click += PointPaintingPanel_Clear;
            pointPaintingPanel.PaintingModeEnabledButton.Click += PointPaintingPanel_PaintingModeEnabled;
            pointPaintingPanel.SelectionModeEnabledButton.Click += PointPaintingPanel_SelectionModeEnabled;
            pointPaintingPanel.Visible = false;

            pointPolygons.Add(currentPointPolygons);

            pointPaintingPanel.Location = new Point(
                (int)(Size.Width - pointPaintingPanel.Width - Size.Width * 0.1),
                (int)(Size.Height * 0.1));

            pointPaintingPanel.KeyDown += EditorView_KeyDown;

            pointPaintingPanel.Show();
            pointPaintingPanel.Visible = false;
        }

        private void DisablePainting()
        {
            paintPointsRequested = false;
            pointsPainting = false;
        }

        private void ShowPointPaintingView()
        {
            DisablePainting();
            pointPaintingPanel.Visible = true;
        }

        private void HidePointPaintingView()
        {
            DisablePainting();
            pointPaintingPanel.Visible = false;
        }

        private void PointPaintingPanel_Clear(object sender, EventArgs e)
        {
            pointPolygons.Clear();
        }

        private void PointPaintingPanel_SelectionModeEnabled(object sender, EventArgs e)
        {
            DisablePainting();
        }

        private void PointPaintingPanel_PaintingModeEnabled(object sender, EventArgs e)
        {
            paintPointsRequested = true;
        }

        #endregion Painting

        private void Upload()
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

                var screenshot = GetScreenShotWithPainting();
                UploadManager.RunUploaderView(screenshot);
            }
        }

        private Bitmap GetScreenShotWithPainting()
        {
            using (var pix = skScreenImage.PeekPixels())
            {

                using (var surface = SKSurface.Create(pix))
                {
                    PaintPoints(surface);

                    SKRectI rect = default;
                    rect.Location = new SKPointI((int)selectionRectangle.X, (int)selectionRectangle.Y);
                    rect.Size = new SKSizeI((int)selectionRectangle.Size.Width, (int)selectionRectangle.Size.Height);

                    using (var shapshot = surface.Snapshot(rect))
                    {
                        using (var data = shapshot.Encode())
                        {
                            using (var stream = data.AsStream())
                            {
                                return new Eto.Drawing.Bitmap(stream);
                            }
                        }
                    }
                }
            }
        }

        public new void Dispose()
        {
            if (disposed == false)
            {
                disposed = true;
                timer?.Dispose();
                etoScreenImage?.Dispose();
                skScreenImage?.Dispose();
                skcontrol?.Dispose();
                pointPaintingPanel?.Close();
            }
        }
    }
}
