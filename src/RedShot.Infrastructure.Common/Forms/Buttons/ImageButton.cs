using System;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    public class ImageButton : Panel
    {
        private Button baseButton;

        public event EventHandler<EventArgs> Clicked;

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

            baseButton.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    baseButton
                }
            };
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
