using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using Saritasa.Tools.Common.Utils;
using SkiaSharp;
using RedShot.Infrastructure.Painting.States;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Painting.PaintingActions;
using RedShot.Infrastructure.Painting.PaintingActions.TextInput;
using RedShot.Infrastructure.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Painting
{
    /// <summary>
    /// Image panel.
    /// </summary>
    internal class ImagePanel : Panel
    {
        /// <summary>
        /// Shows true if the user has added new changes to their picture.
        /// </summary>
        public bool Uploaded { get; set; }

        private readonly SKBitmap image;
        private SKControl skControl;
        private UITimer renderTimer;
        private readonly List<IPaintingAction> paintingActions;
        private readonly List<IPaintingAction> previousPaintingActions;
        private SKBitmap cachedImage;
        private IPaintingAction currentAction;
        private PaintingState paintingState;
        private bool painting;
        private SKPaint skPaint;
        private Cursor eraseCursor;
        private TextInputView textInputView;
        private bool textInputtingEnabled;

        /// <summary>
        /// Initializes image panel via Bitmap image.
        /// </summary>
        public ImagePanel(Bitmap image)
        {
            Size = image.Size;

            InitializeComponents();

            this.image = SkiaSharpHelper.ConvertFromEtoBitmap(image);
            paintingActions = new List<IPaintingAction>();
            previousPaintingActions = new List<IPaintingAction>();
            paintingState = PaintingState.None;

            Shown += ImagePanel_Shown;
        }

        /// <summary>
        /// Changes painting action (Drawing line, rectangle, erasing).
        /// </summary>
        public void ChangePaintingState(PaintingState paintingState)
        {
            if (this.paintingState != paintingState)
            {
                this.paintingState = paintingState;
                ChangeMouseCursor();
            }
        }

        /// <summary>
        /// Gives painting image.
        /// </summary>
        public Bitmap ScreenShot()
        {
            var skInfo = new SKImageInfo(image.Width, image.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(skInfo);
            RenderImage(surface);

            return EtoDrawingHelper.GetEtoBitmapFromSkiaSurface(surface);
        }

        /// <summary>
        /// Changes SkiaSharp paint (Pen).
        /// </summary>
        public void ChangePaint(SKPaint paint)
        {
            skPaint = paint;
            ChangeMouseCursor();
        }

        /// <summary>
        /// Moves back.
        /// Removes last painting action.
        /// </summary>
        public void PaintBack()
        {
            if (painting)
            {
                painting = false;
            }
            else
            {
                paintingActions.Remove(paintingActions.LastOrDefault());
            }
        }

        private void ImagePanel_Shown(object sender, EventArgs e)
        {
            renderTimer.Start();

            this.MouseDown += ImagePanel_MouseDown;
            this.MouseMove += ImagePanel_MouseMove;
            this.MouseUp += ImagePanel_MouseUp;
        }

        private void ChangeMouseCursor()
        {
            switch (paintingState)
            {
                case PaintingState.Points:
                    Cursor = FormsHelper.GetPointerCursor(skPaint.Color,
                        (int)skPaint.StrokeWidth > 3 ? (int)skPaint.StrokeWidth : 4);
                    break;
                case PaintingState.Rectangle:
                case PaintingState.Arrow:
                    Cursor = Cursors.Crosshair;
                    break;
                case PaintingState.Erase:
                    Cursor = eraseCursor;
                    break;
                case PaintingState.Text:
                    Cursor = Cursors.IBeam;
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        private void ImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (painting && currentAction.PaintingActionType == PaintingActionType.MousePainting)
                {
                    FinishPaintingAction();
                }
            }
        }

        private void FinishPaintingAction()
        {
            painting = false;
            paintingActions.Add(currentAction);
        }

        private void ImagePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting)
            {
                currentAction.InputUserAction(new MouseInputAction((Point)e.Location));
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (textInputtingEnabled)
                {
                    textInputView?.Close();
                    textInputtingEnabled = false;
                    FinishPaintingAction();
                }

                if (painting == false && paintingState != PaintingState.None)
                {
                    StartPaintingAction((Point)e.Location);
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                PaintBack();
            }
        }

        private void StartPaintingAction(Point startPoint)
        {
            textInputView?.Close();
            currentAction = PaintingActionsService.MapFromState(paintingState, skPaint.Clone(), image);
            currentAction.AddStartPoint(startPoint);
            painting = true;

            if (currentAction.PaintingActionType == PaintingActionType.KeyboardPainting)
            {
                textInputView = new TextInputView(currentAction);
                textInputtingEnabled = true;
                textInputView.Show();
            }
        }

        private void RenderTimer_Elapsed(object sender, EventArgs e)
        {
            skControl.Execute((surface) => RenderImage(surface));
        }

        private void RenderImage(SKSurface surface)
        {
            var canvas = surface.Canvas;

            if (PaintActionsWithCaching(surface))
            {
                canvas.DrawBitmap(cachedImage, new SKPoint(0, 0));
            }
            else
            {
                canvas.DrawBitmap(image, new SKPoint(0, 0));

                paintingActions.ForEach(a => a.Paint(surface));
                Uploaded = false;
            }

            if (painting)
            {
                currentAction.Paint(surface);
            }
        }

        private bool PaintActionsWithCaching(SKSurface surface)
        {
            if (paintingActions.Count == 0)
            {
                return false;
            }

            var diffResult = CollectionUtils.Diff(previousPaintingActions, paintingActions, (a1, a2) => a1 == a2);

            if (diffResult.Removed.Count == 0)
            {
                if (diffResult.Added.Count > 0)
                {
                    surface.Canvas.DrawBitmap(image, new SKPoint(0, 0));
                    paintingActions.ForEach(a => a.Paint(surface));
                    previousPaintingActions.AddRange(diffResult.Added);
                    cachedImage = SKBitmap.FromImage(surface.Snapshot());
                }

                if (cachedImage != null)
                {
                    return true;
                }
            }
            else
            {
                previousPaintingActions.Clear();
            }

            return false;
        }

        private void InitializeComponents()
        {
            var eraseImage = new Bitmap(Resources.Properties.Resources.EraserPointer);
            eraseCursor = FormsHelper.GetCursor(eraseImage, new Size(20, 20), new Point(5, 5));

            renderTimer = new UITimer()
            {
                Interval = 0.01
            };
            renderTimer.Elapsed += RenderTimer_Elapsed;

            skControl = new SKControl();
            Content = skControl;
        }
    }
}
