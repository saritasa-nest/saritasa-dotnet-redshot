using System;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Helpers.Forms
{
    public class ImageButton : Panel
    {
        public event EventHandler<EventArgs> Clicked;

        public ImageButton(Size size, Bitmap image, string text = null, Size scaleImageSize = default)
        {
            var btn = new Button();
            btn.Width = size.Width;
            btn.Height = size.Height;

            if (scaleImageSize == default)
            {
                scaleImageSize = new Size(Convert.ToInt32(size.Width * 0.6), Convert.ToInt32(size.Height * 0.6));
            }

            btn.Image = new Bitmap(image, scaleImageSize.Width, scaleImageSize.Height, ImageInterpolation.High);

            btn.ImagePosition = ButtonImagePosition.Above;

            if (!string.IsNullOrEmpty(text))
            {
                btn.Text = text;
            }

            btn.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    btn
                }
            };
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
