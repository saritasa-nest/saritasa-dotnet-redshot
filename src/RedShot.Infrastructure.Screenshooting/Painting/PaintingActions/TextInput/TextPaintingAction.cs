using System;
using Eto.Drawing;
using SkiaSharp;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.UserInputActions;

namespace RedShot.Infrastructure.Screenshooting.Painting.PaintingActions.TextInput
{
    /// <summary>
    /// Text painting action.
    /// </summary>
    internal class TextPaintingAction : IPaintingAction
    {
        private Point startPoint;
        private TextInputAction textInputAction;

        /// <inheritdoc />
        public PaintingActionType PaintingActionType => PaintingActionType.KeyboardPainting;

        /// <inheritdoc />
        public void InputUserAction(IInputAction inputAction)
        {
            if (inputAction is TextInputAction textAction)
            {
                textInputAction = textAction;
            }
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
                var point = new SKPoint(startPoint.X, startPoint.Y + 5 + i * textInputAction.TextFont.Size);
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