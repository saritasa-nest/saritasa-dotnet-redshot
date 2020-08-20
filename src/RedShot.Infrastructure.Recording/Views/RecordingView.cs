using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    public class RecordingView : Form
    {
        private Control managePanel;
        private IRecorder recorder;
        private Rectangle recordingRectangle;
        private UITimer labelRenderTimer;
        private Stopwatch recordingTimer;

        private RecordingButton recordingButton;
        private DefaultButton closeButton;
        private Label timerLabel;

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

            recordingTimer = new Stopwatch();

            labelRenderTimer = new UITimer();
            labelRenderTimer.Interval = 0.01;
            labelRenderTimer.Elapsed += RecordingLabelTimer_Elapsed;
            labelRenderTimer.Start();

            this.Closed += RecordingView_Closed;
        }

        private void RecordingView_Closed(object sender, EventArgs e)
        {
            recorder?.Stop();
        }

        private void RecordingButton_Clicked(object sender, System.EventArgs e)
        {
            if (recordingButton.IsRecording)
            {
                StopRecording();
            }
            else
            {
                recordingTimer.Reset();
                recorder.Start(recordingRectangle.OffsetRectangle(1));

                while (!recorder.IsRecording)
                {
                }

                recordingTimer.Start();
            }
        }

        private void CloseButton_Clicked(object sender, System.EventArgs e)
        {
            recorder.Stop();
            Close();
        }

        private void StopRecording()
        {
            recordingTimer.Stop();
            recorder.Stop();

            UploadManager.RunUploading(recorder.GetVideo());
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
            Size = new Size(managePanel.Width, managePanel.Height);
#endif
            }

        private void InitializeComponents()
        {
            timerLabel = new Label()
            {
                Text = TimeSpan.Zero.ToString()
            };

            recordingButton = new RecordingButton(60, 50);
            recordingButton.Clicked += RecordingButton_Clicked;

            closeButton = new DefaultButton("Close", 60, 50);
            closeButton.Clicked += CloseButton_Clicked;

            managePanel = GetManageControl();
        }

        private Control GetManageControl()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 3,
                BackgroundColor = Colors.White,
                Size = new Size(240, 54),
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
