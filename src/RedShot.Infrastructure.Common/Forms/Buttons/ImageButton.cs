using System;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Image button.
    /// </summary>
    public class ImageButton : Panel
    {
        private Button baseButton;

        /// <summary>
        /// Event to handle when the user clicks the button.
        /// </summary>
        public event EventHandler<EventArgs> Clicked;

        /// <summary>
        /// Tool tip of the button.
        /// </summary>
        public override string ToolTip
        {
            get
            {
                return baseButton?.ToolTip;
            }

            set
            {
                baseButton.ToolTip = value;
            }
        }

        /// <summary>
        /// Initializes image button.
        /// </summary>
        public ImageButton(Size size, Bitmap image, string text = null, Size scaleImageSize = default)
        {
            baseButton = new Button();
            baseButton.Width = size.Width;
            baseButton.Height = size.Height;

            if (scaleImageSize == default)
            {
                scaleImageSize = new Size(Convert.ToInt32(size.Width * 0.6), Convert.ToInt32(size.Height * 0.6));
            }

            baseButton.Image = new Bitmap(image, scaleImageSize.Width, scaleImageSize.Height, ImageInterpolation.High);

            baseButton.ImagePosition = ButtonImagePosition.Above;

            if (!string.IsNullOrEmpty(text))
            {
                baseButton.Text = text;
            }

            baseButton.Click += BaseButtonClick;
            Content = baseButton;
        }

        /// <inheritdoc/>
        public override void Focus()
        {
            base.Focus();
            baseButton.Focus();
        }

        private void BaseButtonClick(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
