﻿using Eto.Drawing;

namespace RedShot.Infrastructure.Painting.PaintingActions.UserInputActions
{
    /// <summary>
    /// Text input action.
    /// </summary>
    internal class TextInputAction : IInputAction
    {
        /// <summary>
        /// Initializes text input action.
        /// </summary>
        public TextInputAction(string text, Font textFont, Color textColor)
        {
            Text = text;
            TextFont = textFont;
            TextColor = textColor;
        }

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Text font.
        /// </summary>
        public Font TextFont { get; }

        /// <summary>
        /// Text color.
        /// </summary>
        public Color TextColor { get; }
    }
}