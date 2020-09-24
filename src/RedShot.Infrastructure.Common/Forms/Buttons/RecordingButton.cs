using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Resources;

namespace RedShot.Infrastructure.Common.Forms
{
    /// <summary>
    /// Provides button for recording.
    /// </summary>
    public class RecordingButton : Panel
    {
        /// <summary>
        /// Event to handle when the user clicks the button.
        /// </summary>
        public event EventHandler<EventArgs> Clicked;

        /// <summary>
        /// State of the button.
        /// </summary>
        public bool IsRecording { get; private set; }

        private readonly Bitmap playIcon;
        private readonly Bitmap stopIcon;
        private Button button;

        /// <summary>
        /// Initializes recording button.
        /// </summary>
        public RecordingButton(int width, int height)
        {
            Width = width;
            Height = height;

            playIcon = Icons.Play;
            stopIcon = Icons.Stop;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            button = new Button
            {
                Width = Width,
                Height = Height
            };

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

        /// <summary>
        /// Reverts state of the button.
        /// </summary>
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
            button.Image = new Bitmap(image, Convert.ToInt32(Width * 0.7), Convert.ToInt32(Height * 0.7), ImageInterpolation.High);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
            RevertState();
        }
    }
}
