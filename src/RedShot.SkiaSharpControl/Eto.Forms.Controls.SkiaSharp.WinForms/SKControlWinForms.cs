﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Eto.Forms.Controls.SkiaSharp.WinForms
{
    /// <summary>
    /// SkiaSharp drawing control for WIN OS.
    /// </summary>
    public class SKControlWinForms : global::SkiaSharp.Views.Desktop.SKControl
    {
        private Bitmap bitmap;
        private SKSurface surface;

        /// <summary>
        /// Executes SkiaSharp commands.
        /// </summary>
        public void Execute(Action<SKSurface> surfaceAction)
        {
            if (bitmap == null)
            {
                CreateBitmap();
            }

            var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            var info = new SKImageInfo(Width, Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            if (surface == null)
            {
                surface = SKSurface.Create(info, data.Scan0, data.Stride);
            }

            surfaceAction.Invoke(surface);
            OnPaintSurface(new SKPaintSurfaceEventArgs(surface, info));

            surface.Canvas.Flush();

            bitmap.UnlockBits(data);
            Refresh();
        }

        /// <inheritdoc />
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (bitmap != null)
            {
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
        }

        private void CreateBitmap()
        {
            if (bitmap == null || bitmap.Width != Width || bitmap.Height != Height)
            {
                FreeBitmap();

                bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            }
        }

        private void FreeBitmap()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        /// <inheritdoc />
        public new void Dispose()
        {
            if (IsDisposed == false)
            {
                surface?.Dispose();
                bitmap?.Dispose();
                base.Dispose();
            }
        }
    }
}
