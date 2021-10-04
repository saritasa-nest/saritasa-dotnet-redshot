using System;
using System.Runtime.InteropServices;
using MonoMac.CoreGraphics;
using SkiaSharp;

namespace Eto.Forms.Controls.SkiaSharp.Mac
{
    /// <summary>
    /// Represents helper to work with MAC control.
    /// </summary>
    internal class SKDrawable : IDisposable
    {
        private const int BitsPerByte = 8;
        private const CGBitmapFlags BitmapFlags = CGBitmapFlags.ByteOrder32Big | CGBitmapFlags.PremultipliedLast;
        private IntPtr bitmapData;
        private int lastLength;

        /// <summary>
        /// SKImage info property.
        /// </summary>
        public SKImageInfo Info { get; private set; }

        /// <summary>
        /// Creates surface via content bounds and scale.
        /// </summary>
        public SKSurface CreateSurface(CGRect contentsBounds, float scale, out SKImageInfo info)
        {
            // Apply a scale.
            contentsBounds.Width *= scale;
            contentsBounds.Height *= scale;

            // Get context details.
            info = new SKImageInfo((int)contentsBounds.Width, (int)contentsBounds.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            Info = info;

            // Allocate a memory block for the drawing process.
            var newLength = info.BytesSize;
            if (lastLength != newLength)
            {
                lastLength = newLength;
                if (bitmapData != IntPtr.Zero)
                {
                    bitmapData = Marshal.ReAllocCoTaskMem(bitmapData, newLength);
                }
                else
                {
                    bitmapData = Marshal.AllocCoTaskMem(newLength);
                }
            }

            return SKSurface.Create(info, bitmapData, info.RowBytes);
        }

        /// <summary>
        /// Draws surface on CG context.
        /// </summary>
        public void DrawSurface(CGContext ctx, CGRect viewBounds, SKImageInfo info, SKSurface surface)
        {
            surface.Canvas.Flush();

            // draw the image onto the context
            using var dataProvider = new CGDataProvider(bitmapData, lastLength);
            using var colorSpace = CGColorSpace.CreateDeviceRGB();
            using var image = new CGImage(info.Width, info.Height, BitsPerByte, info.BytesPerPixel * BitsPerByte, info.RowBytes, colorSpace, BitmapFlags, dataProvider, null, false, CGColorRenderingIntent.Default);
            // Draw the image.
            ctx.DrawImage(viewBounds, image);
        }

        /// <summary>
        /// Disposes bitmap data.
        /// </summary>
        public void Dispose()
        {
            // Make sure we free the image data.
            if (bitmapData != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(bitmapData);
                bitmapData = IntPtr.Zero;
            }
        }
    }
}
