﻿using Eto.Drawing;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Abstractions.Painting;
using RedShot.App.Painting.PaintingActions;
using RedShot.App.Painting.States;
using RedShot.Helpers;
using Saritasa.Tools.Common.Utils;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedShot.App.Painting
{
    internal class ImagePanel : Panel
    {
        private readonly SKBitmap image;
        private SKControl skControl;
        private UITimer renderTimer;

        private List<IPaintingAction> paintingActions;

        private List<IPaintingAction> previousPaintingActions;
        private SKImage cachedImage;

        private IPaintingAction currentAction;
        public PaintingState PaintingState { get; set; }

        Cursor paintingPointer;

        private bool painting;
        private SKPaint skPaint;

        public ImagePanel(Bitmap image)
        {
            Size = image.Size;

            InitializeComponents();

            this.image = SkiaSharpHelper.ConvertFromEtoBitmap(image);
            paintingActions = new List<IPaintingAction>();
            previousPaintingActions = new List<IPaintingAction>();

            this.MouseDown += ImagePanel_MouseDown;
            this.MouseMove += ImagePanel_MouseMove;
            this.MouseUp += ImagePanel_MouseUp;
            Shown += ImagePanel_Shown;
        }

        private void ImagePanel_Shown(object sender, EventArgs e)
        {
            renderTimer.Start();
        }

        private void ImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                paintingActions.Add(currentAction);
                painting = false;
                previousPaintingActions.Clear();
            }
        }

        private void ImagePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting)
            {
                currentAction.AddPoint(new Point(e.Location));
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                if (painting)
                {
                    painting = false;
                }
                else
                {
                    if (PaintingState != PaintingState.None)
                    {
                        SetAction();
                        painting = true;
                        currentAction.AddPoint(new Point(e.Location));
                    }
                }
            }
            else if (e.Buttons == MouseButtons.Alternate)
            {
                PaintBack();
            }
        }

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

        private void SetAction()
        {
            currentAction = PaintingActionsService.MapFromState(PaintingState, skPaint.Clone(), image);
        }

        private void InitializeComponents()
        {
            renderTimer = new UITimer()
            {
                Interval = 0.01
            };
            renderTimer.Elapsed += RenderTimer_Elapsed;

            skControl = new SKControl();
            Content = skControl;
        }

        private void RenderTimer_Elapsed(object sender, EventArgs e)
        {
            skControl.Execute((surface) => RenderImage(surface));
        }

        private void RenderImage(SKSurface surface)
        {
            var canvas = surface.Canvas;

            if (PaintActionsWithCaching(surface) && cachedImage != null)
            {
                canvas.DrawImage(cachedImage, new SKPoint(0, 0));
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
                    foreach (var action in diffResult.Added)
                    {
                        action.Paint(surface);
                    }
                    previousPaintingActions.AddRange(diffResult.Added);

                    cachedImage = surface.Snapshot();
                }

                return true;
            }
            else
            {
                previousPaintingActions.Clear();
            }

            return false;
        }

        public Bitmap ScreenShot()
        {
            using var surface = SKSurface.Create(image.Width, image.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            RenderImage(surface);

            return EtoDrawingHelper.GetEtoBitmapFromSkiaSurface(surface);
        }

        public void ChangePaint(SKPaint paint)
        {
            skPaint = paint;
        }
    }
}
