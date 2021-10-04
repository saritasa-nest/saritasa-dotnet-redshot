﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Drawing;
using SkiaSharp;

namespace RedShot.Eto.Desktop.Helpers
{
    /// <summary>
    /// Helpers for working with Eto drawing.
    /// </summary>
    public static class EtoDrawingHelper
    {
        /// <summary>
        /// Creates rectangle via two points.
        /// </summary>
        public static RectangleF CreateRectangle(PointF startLocation, PointF endLocation)
        {
            float width, height;
            float x, y;

            if (startLocation.X <= endLocation.X)
            {
                width = endLocation.X - startLocation.X + 1;
                x = startLocation.X;
            }
            else
            {
                width = startLocation.X - endLocation.X + 1;
                x = endLocation.X;
            }

            if (startLocation.Y <= endLocation.Y)
            {
                height = endLocation.Y - startLocation.Y + 1;
                y = startLocation.Y;
            }
            else
            {
                height = startLocation.Y - endLocation.Y + 1;
                y = endLocation.Y;
            }

            return new RectangleF(x, y, width, height);
        }

        /// <summary>
        /// Converts Skia surface to Eto bitmap.
        /// </summary>
        public static Bitmap GetEtoBitmapFromSkiaSurface(SKSurface surface)
        {
            using var shapshot = surface.Snapshot();
            return GetEtoBitmapFromSkiaImage(shapshot);
        }

        /// <summary>
        /// Converts Skia image to Eto bitmap.
        /// </summary>
        public static Bitmap GetEtoBitmapFromSkiaImage(SKImage skImage)
        {
            using var data = skImage.Encode();
            using var stream = data.AsStream();
            stream.Seek(0, SeekOrigin.Begin);
            return new Bitmap(stream);
        }

        public static Bitmap GetEtoBitmapFromDrawing(System.Drawing.Bitmap drawingBitmap)
        {
            using var memoryStream = new MemoryStream();
            drawingBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new Bitmap(memoryStream);
        }

        /// <summary>
        /// Offsets rectangle.
        /// </summary>
        public static Rectangle OffsetRectangle(this Rectangle rect, int i)
        {
            return new Rectangle(rect.X + i, rect.Y + i, rect.Width - i * 2, rect.Height - i * 2);
        }

        /// <summary>
        /// Save bitmap image <see langword="async"/>.
        /// </summary>
        public static async Task SaveAsync(this Bitmap bitmap, string filePath, ImageFormat imageFormat, CancellationToken cancellationToken = default)
        {
            var bytes = bitmap.ToByteArray(imageFormat);
            await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
        }

        /// <summary>
        /// Scale size.
        /// </summary>
        public static Size Scale(this Size size, double scaleRate)
        {
            return new Size(Convert.ToInt32(size.Width * scaleRate), Convert.ToInt32(size.Height * scaleRate));
        }
    }
}
