using System;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    public class ImageButton : Panel
    {
        public event EventHandler<EventArgs> Clicked;

        public ImageButton(Size size, Bitmap image, string text = null, Size scaleImageSize = default)
        {
            var button = new Button();
            button.Width = size.Width;
            button.Height = size.Height;

            if (scaleImageSize == default)
            {
                scaleImageSize = new Size(Convert.ToInt32(size.Width * 0.6), Convert.ToInt32(size.Height * 0.6));
            }

            button.Image = new Bitmap(image, scaleImageSize.Width, scaleImageSize.Height, ImageInterpolation.High);

            button.ImagePosition = ButtonImagePosition.Above;

            if (!string.IsNullOrEmpty(text))
            {
                button.Text = text;
            }

            button.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    button
                }
            };
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
