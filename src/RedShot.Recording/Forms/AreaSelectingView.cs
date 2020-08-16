using System;
using Eto.Drawing;
using Eto.Forms;
using SkiaSharp;
using RedShot.Helpers.EditorView;
using RedShot.Helpers;

namespace RedShot.Recording.Forms
{
    internal partial class AreaSelectingView : Dialog
    {
        /// <summary>
        /// Render frametime in milliseconds.
        /// Should be more than 10 in Linux OS.
        /// </summary>
        private readonly double renderFrameTime = 10;

        public AreaSelectingView()
        {
            InitializeComponent();
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
                    SelectionRectangle = EtoDrawingHelper.CreateRectangle(startLocation, endLocation);
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
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                if (capturing)
                {
                    capturing = false;
                }
                else if (captured)
                {
                    captured = false;
                    moving = false;
                    resizing = false;
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
                case Keys.Escape | Keys.Enter:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;

                case Keys.Enter:
                    if (captured)
                    {
                        DialogResult = DialogResult.Ok;
                        Close();
                    }
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

            var text = $"X: {FormatNumber(SelectionRectangle.X)} Y: {FormatNumber(SelectionRectangle.Y)}" +
                $"   W: {FormatNumber(SelectionRectangle.Width)} H: {FormatNumber(SelectionRectangle.Height)}";
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

            if (SelectionRectangle.Y > paint.TextSize + 10)
            {
                drawCoords = new SKPoint(SelectionRectangle.X + 2, SelectionRectangle.Y - 15);
            }
            else if (SelectionRectangle.Y + SelectionRectangle.Height < Height - paint.TextSize - 10)
            {
                drawCoords = new SKPoint(SelectionRectangle.X + 2, SelectionRectangle.Y + SelectionRectangle.Height + 20);
            }
            else
            {
                drawCoords = new SKPoint(SelectionRectangle.X + 6, SelectionRectangle.Y + paint.TextSize + 6);
            }

            textStrokeRect.Location = new SKPoint(drawCoords.X - 2, drawCoords.Y - paint.TextSize);
            textStrokeRect.Size = new SKSize(textWidth + 4, paint.TextSize + 4);

            textfillRect.Location = new SKPoint(textStrokeRect.Location.X + 1, textStrokeRect.Location.Y + 1);
            textfillRect.Size = new SKSize(textStrokeRect.Width - 1, textStrokeRect.Height - 1);

            canvas.DrawRect(textfillRect, fillRectPaint);
            canvas.DrawRect(textStrokeRect, strokeRectPaint);

            surface.Canvas.DrawText(text, drawCoords, paint);
        }

        private string FormatNumber(float number)
        {
            return number.ToString("F0");
        }

        private void PaintClearImage(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));

            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width - 1, Height - 1));

            PaintDarkregion(surface, editorRect);

            PaintTopMessage(surface);
        }

        private void PaintRegion(SKSurface surface)
        {
            var canvas = surface.Canvas;

            canvas.DrawBitmap(skScreenImage, new SKPoint(0, 0));

            var size = new SKSize(SelectionRectangle.Width, SelectionRectangle.Height);
            var point = new SKPoint(SelectionRectangle.X, SelectionRectangle.Y);

            var regionRect = SKRect.Create(point, size);

            var editorRect = SKRect.Create(new SKPoint(0, 0), new SKSize(Width, Height));

            PaintDarkregion(surface, editorRect, regionRect);

            PaintDashAround(surface, regionRect, SKColors.Red, SKColors.Black);

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

        #region MovingResizing

        #region Checking

        /// <summary>
        /// Checks whether user can move selected area.
        /// </summary>
        private bool CheckOnMoving(PointF mouseLocation)
        {
            if (mouseLocation.X >= SelectionRectangle.X && mouseLocation.X <= SelectionRectangle.X + SelectionRectangle.Width)
            {
                if (mouseLocation.Y >= SelectionRectangle.Y && mouseLocation.Y <= SelectionRectangle.Y + SelectionRectangle.Height)
                {
                    relativeX = mouseLocation.X - SelectionRectangle.X;
                    relativeY = mouseLocation.Y - SelectionRectangle.Y;

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
            if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.Y, mouseLocation.Y))
            {
                if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y + SelectionRectangle.Height);
                }
                else if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X + SelectionRectangle.Width, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(SelectionRectangle.X, SelectionRectangle.Y + SelectionRectangle.Height);
                }
                else if (mouseLocation.X > SelectionRectangle.X && mouseLocation.X < SelectionRectangle.X + SelectionRectangle.Width)
                {
                    resizePart = ResizePart.HorizontalBorder;

                    var start = new PointF(SelectionRectangle.X, SelectionRectangle.Y + SelectionRectangle.Height);
                    var end = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y + SelectionRectangle.Height);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.Y + SelectionRectangle.Height, mouseLocation.Y))
            {
                if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y);
                }
                else if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X + SelectionRectangle.Width, mouseLocation.X))
                {
                    resizePart = ResizePart.Angle;
                    oppositeAngle = new PointF(SelectionRectangle.X, SelectionRectangle.Y);
                }
                else if (mouseLocation.X > SelectionRectangle.X && mouseLocation.X < SelectionRectangle.X + SelectionRectangle.Width)
                {
                    resizePart = ResizePart.HorizontalBorder;

                    var start = new PointF(SelectionRectangle.X, SelectionRectangle.Y);
                    var end = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X, mouseLocation.X))
            {
                if (mouseLocation.Y > SelectionRectangle.Y && mouseLocation.Y < SelectionRectangle.Y + Height)
                {
                    resizePart = ResizePart.VerticalBorder;

                    var start = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y);
                    var end = new PointF(SelectionRectangle.X + SelectionRectangle.Width, SelectionRectangle.Y + SelectionRectangle.Height);
                    oppositeBorder = new LineF(start, end);
                }
                else
                {
                    return false;
                }
            }
            else if (ResizeHelper.ApproximatelyEquals(SelectionRectangle.X + SelectionRectangle.Width, mouseLocation.X))
            {
                if (mouseLocation.Y > SelectionRectangle.Y && mouseLocation.Y < SelectionRectangle.Y + Height)
                {
                    resizePart = ResizePart.VerticalBorder;

                    var start = new PointF(SelectionRectangle.X, SelectionRectangle.Y);
                    var end = new PointF(SelectionRectangle.X, SelectionRectangle.Y + SelectionRectangle.Height);
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
                if (newXcoord + SelectionRectangle.Width < Size.Width)
                {
                    SelectionRectangle.X = newXcoord;
                }
                else
                {
                    SelectionRectangle.X = Size.Width - SelectionRectangle.Width;
                }

                if (newYcoord + SelectionRectangle.Height <= Size.Height)
                {
                    SelectionRectangle.Y = newYcoord;
                }
                else
                {
                    SelectionRectangle.Y = Size.Height - SelectionRectangle.Height;
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
                SelectionRectangle = EtoDrawingHelper.CreateRectangle(location, oppositeAngle);
            }
            else if (resizePart == ResizePart.HorizontalBorder)
            {
                var point = new PointF(oppositeBorder.StartPoint.X, location.Y);
                SelectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
            }
            else if (resizePart == ResizePart.VerticalBorder)
            {
                var point = new PointF(location.X, oppositeBorder.StartPoint.Y);
                SelectionRectangle = EtoDrawingHelper.CreateRectangle(point, oppositeBorder.EndPoint);
            }
        }

        #endregion MovingResizing
    }
}
