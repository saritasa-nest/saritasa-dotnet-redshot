using System;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Forms
{
    public class RecordingButton : Panel
    {
        public event EventHandler<EventArgs> Clicked;

        public bool IsRecording { get; private set; }

        private Bitmap playIcon;
        private Bitmap stopIcon;
        private Button button;

        public RecordingButton(int width, int height)
        {
            Width = width;
            Height = height;

            playIcon = new Bitmap(Resources.Properties.Resources.Play);
            stopIcon = new Bitmap(Resources.Properties.Resources.Stop);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            button = new Button();
            button.Width = Width;
            button.Height = Height;

            SetImage(playIcon);

            button.Click += Btn_Click;

            Content = new StackLayout
            {
                Items =
                {
                    button
                }
            };
        }

        public void RevertState()
        {
            if (IsRecording)
            {
                IsRecording = false;
                SetImage(playIcon);
            }
            else
            {
                IsRecording = true;
                SetImage(stopIcon);
            }

            Invalidate(true);
        }

        private void SetImage(Bitmap image)
        {
            var scaleImageSize = new Size(Convert.ToInt32(Width * 0.7), Convert.ToInt32(Width * 0.7));

            button.Image = new Bitmap(image, scaleImageSize.Width, scaleImageSize.Height, ImageInterpolation.High);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
            RevertState();
        }
    }
}
