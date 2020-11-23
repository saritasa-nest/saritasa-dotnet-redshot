using System;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Common;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextActions
{
    /// <summary>
    /// Text painting action.
    /// </summary>
    internal class TextPaintingAction : IPaintingAction
    {
        private Point startPoint;
        private TextInputAction textInputAction;

        /// <summary>
        /// Input text action.
        /// </summary>
        public void InputTextAction(TextInputAction textAction)
        {
            textInputAction = textAction;
        }

        /// <inheritdoc />
        public void Paint(SKSurface surface)
        {
            if (textInputAction == null)
            {
                return;
            }

            var lines = textInputAction.Text.Split(new[]
            {
                "\n", "\r\n", "\r"
            }, StringSplitOptions.None);
            var paint = GetPaintByInput(textInputAction);

            for (var i = 0; i < lines.Length; i++)
            {
                var point = new SKPoint(startPoint.X, startPoint.Y + i * textInputAction.TextFont.Size);
                surface.Canvas.DrawText(lines[i], point, paint);
            }
        }

        /// <inheritdoc />
        public void AddStartPoint(Point point)
        {
            startPoint = point;
        }

        private SKPaint GetPaintByInput(TextInputAction inputAction)
        {
            var paint = new SKPaint
            {
                TextSize = inputAction.TextFont.Size,
                Color = SkiaSharpHelper.GetSKColorFromEtoColor(inputAction.TextColor),
                Typeface = SKTypeface.FromFamilyName(
                    inputAction.TextFont.FamilyName,
                    inputAction.TextFont.Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright)
            };

            return paint;
        }
    }
}