using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using Saritasa.Tools.Common.Utils;
using SkiaSharp;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Resources;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions;
using RedShot.Infrastructure.Screenshooting.Painting.States;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.MouseActions;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextActions;

namespace RedShot.Infrastructure.Screenshooting.Painting
{
    /// <summary>
    /// Image panel.
    /// </summary>
    internal class ImagePanel : Panel
    {
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
        }

        /// <summary>
        /// Get image hash.
        /// </summary>
        public int GetImageHash()
        {
            int hash = this.GetHashCode();

            foreach (var action in paintingActions)
            {
                hash = HashCode.Combine(hash, action);
            }

            return hash;
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
        /// Get painting image.
        /// </summary>
        public Bitmap GetPaintingImage()
        {
            var skInfo = new SKImageInfo(image.Width, image.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(skInfo);
            RenderImage(surface);

            return EtoDrawingHelper.GetEtoBitmapFromSkiaSurface(surface);
        }

        /// <summary>
        /// Change SkiaSharp paint.
        /// </summary>
        public void ChangePaint(SKPaint paint)
        {
            skPaint = paint;
            ChangeMouseCursor();
        }

        /// <summary>
        /// Move back.
        /// Remove last painting action.
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Buttons == MouseButtons.Primary)
            {
                if (painting == false && paintingState != PaintingState.None)
                {
                    StartPaintingAction((Point)e.Location);
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                PaintBack();
            }

            e.Handled = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            renderTimer.Start();
        }

        private void ChangeMouseCursor()
        {
            switch (paintingState)
            {
                case PaintingState.Brush:
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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Buttons == MouseButtons.Primary)
            {
                if (painting && currentAction is IMousePaintingAction)
                {
                    painting = false;
                    paintingActions.Add(currentAction);
                }
            }

            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (painting && currentAction is IMousePaintingAction mouseAction)
            {
                mouseAction.InputMouseAction((Point)e.Location);
            }

            e.Handled = true;
        }

        private void StartPaintingAction(Point startPoint)
        {
            var action = PaintingActionsMappingService.MapFromState(paintingState, skPaint.Clone(), image);
            action.AddStartPoint(startPoint);

            if (action is TextPaintingAction textAction)
            {
                using var textDialog = new TextInputDialog();
                var result = textDialog.ShowModal();

                if (result.DialogResult == DialogResult.Ok)
                {
                    textAction.InputTextAction(result.TextInputAction);
                    paintingActions.Add(textAction);
                }
                FindParent<Control>().Invalidate(true);
                FindParent<Control>().Focus();
            }
            else
            {
                currentAction = action;
                painting = true;
            }
        }

        private void RenderTimerElapsed(object sender, EventArgs e)
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
            var eraseImage = Icons.EraserPointer;
            eraseCursor = FormsHelper.GetCursor(eraseImage, new Size(20, 20), new Point(5, 5));

            renderTimer = new UITimer()
            {
                Interval = 0.01
            };
            renderTimer.Elapsed += RenderTimerElapsed;

            skControl = new SKControl();
            Content = skControl;
        }
    }
}
