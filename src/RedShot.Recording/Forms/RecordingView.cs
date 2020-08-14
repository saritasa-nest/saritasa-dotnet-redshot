using Eto.Drawing;
using Eto.Forms;
using RedShot.Helpers;
using RedShot.Helpers.Forms;
using RedShot.Recording.Recorders;
using System.Diagnostics;

namespace RedShot.Recording.Forms
{
    public class RecordingView : Form
    {
        private Control managePanel;
        private IRecorder recorder;
        private Rectangle recordingRectangle;
        private UITimer recordingLabelTimer;
        private Stopwatch recordingTimer = new Stopwatch();

        private RecordingButton recordingButton;
        private DefaultButton closeButton;
        private Label timerLabel = new Label();

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

            SetupLocations();

            BackgroundColor = Colors.Red;

            recordingLabelTimer = new UITimer();
            recordingLabelTimer.Interval = 0.01;
            recordingLabelTimer.Elapsed += RecordingLabelTimer_Elapsed;
            recordingLabelTimer.Start();
        }

        private void RecordingLabelTimer_Elapsed(object sender, System.EventArgs e)
        {
            timerLabel.Text = recordingTimer.Elapsed.ToString();
            timerLabel.Invalidate();
        }

        private void SetupLocations()
        {
            if ((recordingRectangle.Location.Y - managePanel.Height) > 0)
            {
                Location = new Point(recordingRectangle.X, recordingRectangle.Y - managePanel.Height - 1);

#if _WINDOWS
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + managePanel.Height + 1);

                var excludeRect = new Rectangle(new Point(0, managePanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);

                var excludeRect2 = new Rectangle(new Point(managePanel.Width, 0), new Size(recordingRectangle.Width - managePanel.Width, managePanel.Height));

                RedShot.Platforms.Windows.WindowsRegionHelper.Exclude(this.ControlObject, excludeRect, excludeRect2);
#endif
            }
            else
            {
                var screenBounds = ScreenHelper.GetScreenSizeByLocation(recordingRectangle.Location);

                if ((recordingRectangle.Location.Y + managePanel.Height) < screenBounds.Height)
                {
                    Location = recordingRectangle.Location;

#if _WINDOWS
                    Size = new Size(recordingRectangle.Width, recordingRectangle.Height + 1);

                    var excludeRect = new Rectangle(new Point(0, managePanel.Height - 1), new Size(recordingRectangle.Width, recordingRectangle.Height - managePanel.Height + 2)).OffsetRectangle(1);

                    var excludeRect2 = new Rectangle(new Point(managePanel.Width, 1), new Size(recordingRectangle.Width - managePanel.Width - 1, managePanel.Height));

                    RedShot.Platforms.Windows.WindowsRegionHelper.Exclude(this.ControlObject, excludeRect, excludeRect2);
#endif
                }
            }

#if _UNIX
            Location = recordingRectangle.Location;
            Size = new Size(managePanel.Width, managePanel.Height);
#endif
        }

        private void InitializeComponents()
        {
            timerLabel = new Label()
            {
                Text = recordingTimer.Elapsed.ToString()
            };

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
                recordingTimer.Stop();
            }
            else
            {
                recorder.Start(recordingRectangle.OffsetRectangle(1));
                recordingTimer.Start();
            }
        }

        private void CloseButton_Clicked(object sender, System.EventArgs e)
        {
            recorder.Stop();
            recordingTimer.Stop();
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
                Size = new Size(220, 40),
                Items =
                {
                    recordingButton,
                    closeButton,
                    FormsHelper.VoidBox(20),
                    timerLabel
                }
            };
        }
    }
}
