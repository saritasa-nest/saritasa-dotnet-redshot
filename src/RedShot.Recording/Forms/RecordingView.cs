using Eto.Drawing;
using Eto.Forms;
using RedShot.Helpers;
using RedShot.Helpers.Forms;
using RedShot.Recording.Recorders;

namespace RedShot.Recording.Forms
{
    public class RecordingView : Form
    {
        private Control managePanel;
        private IRecorder recorder;
        private Rectangle recordingRectangle;

        private RecordingButton recordingButton;
        private DefaultButton closeButton;

        public RecordingView(IRecorder recorder, Rectangle recordingRectangle)
        {
            this.recorder = recorder;
            this.recordingRectangle = recordingRectangle;
            this.Resizable = false;
            this.MovableByWindowBackground = false;

            WindowStyle = WindowStyle.None;
            Topmost = true;

            InitializeComponents();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    managePanel
                }
            };

            Location = new Point(recordingRectangle.X, recordingRectangle.Y - managePanel.Height - 1);

#if _WINDOWS
            Size = new Size(recordingRectangle.Width, recordingRectangle.Height + managePanel.Height + 1);

            var excludeRect = new Rectangle(new Point(0, managePanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);

            var excludeRect2 = new Rectangle(new Point(managePanel.Width, 0), new Size(recordingRectangle.Width - managePanel.Width, managePanel.Height));

            RedShot.Platforms.Windows.WindowsRegionHelper.Exclude(this.ControlObject, excludeRect, excludeRect2);
#elif _UNIX
            Size = new Size(managePanel.Width, managePanel.Height);
#endif
            BackgroundColor = Colors.Red;
        }

        private void InitializeComponents()
        {
            recordingButton = new RecordingButton(50, 35);
            recordingButton.Clicked += RecordingButton_Clicked;

            closeButton = new DefaultButton("Close", 50, 35);
            closeButton.Clicked += CloseButton_Clicked;

            managePanel = GetManageControl();
        }

        private void RecordingButton_Clicked(object sender, System.EventArgs e)
        {
            if (recordingButton.IsRecording)
            {
                recorder.Stop();
            }
            else
            {
                recorder.Start(recordingRectangle.OffsetRectangle(1));
            }
        }

        private void CloseButton_Clicked(object sender, System.EventArgs e)
        {
            recorder.Stop();
            Close();
        }

        private Control GetManageControl()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 3,
                BackgroundColor = Colors.White,
                Height = 40,
                Width = 106,
                Items =
                {
                    recordingButton,
                    closeButton
                }
            };
        }
    }
}
