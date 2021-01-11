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
        private Label countdownLabel;

        /// <summary>
        /// Initializes recording button.
        /// </summary>
        public RecordingButton(Size size)
        {
            Size = size;

            playIcon = Icons.Play;
            stopIcon = Icons.Stop;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            button = new Button
            {
                Size = Size,
                ToolTip = "Start"
            };

            countdownLabel = new Label()
            {
                Font = new Font(FontFamilies.Sans, 14),
                Size = Size.Scale(0.70),
                TextColor = Colors.Red,
                TextAlignment = TextAlignment.Center,
                Visible = false
            };

            SetImage(playIcon);

            button.Click += ButtonClick;

            var content = new PixelLayout();
            content.Add(button, Point.Empty);
            content.Add(countdownLabel, new Point(Size.Scale(0.15)));
            Content = content;
        }

        /// <summary>
        /// Reverts state of the button.
        /// </summary>
        public void RevertState()
        {
            countdownLabel.Visible = false;

            if (IsRecording)
            {
                IsRecording = false;
                SetImage(playIcon);
                button.ToolTip = "Start";
            }
            else
            {
                IsRecording = true;
                SetImage(stopIcon);
                button.ToolTip = "Stop";
            }

            Invalidate(true);
        }

        /// <summary>
        /// Set countdown second.
        /// </summary>
        /// <param name="second">Second.</param>
        public void SetCountdownSecond(int second)
        {
            countdownLabel.Visible = true;
            countdownLabel.Text = second.ToString();
            Invalidate(true);
        }

        private void SetImage(Bitmap image)
        {
            var scaledSize = Size.Scale(0.6);
            button.Image = new Bitmap(image, scaledSize.Width, scaledSize.Height, ImageInterpolation.High);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }
    }
}
