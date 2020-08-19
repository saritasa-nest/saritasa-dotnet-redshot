using System;
using System.Dynamic;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Helpers.Properties;

namespace RedShot.Infrastructure.Common.Forms
{
    public class RecordingButton : Panel
    {
        public event EventHandler<EventArgs> Clicked;

        public bool IsRecording { get; private set; }

        private Bitmap playIcon;
        private Bitmap stopIcon;
        private int width;
        private int height;
        private Button button;

        public RecordingButton(int width, int height)
        {
            this.width = width;
            this.height = height;

            playIcon = new Bitmap(Resources.play);
            stopIcon = new Bitmap(Resources.stop);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            button = new Button();
            button.Width = width;
            button.Height = height;

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
            var scaleImageSize = new Size(Convert.ToInt32(width * 0.7), Convert.ToInt32(width * 0.7));

            button.Image = new Bitmap(image, scaleImageSize.Width, scaleImageSize.Height, ImageInterpolation.High);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
            RevertState();
        }
    }
}
