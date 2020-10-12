using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Uploading;
using RedShot.Resources;

namespace RedShot.Infrastructure.Recording.Views
{
    internal class RecordingVideoPanel : Panel
    {
        public event EventHandler Closed;

        private readonly IRecorder recorder;
        private Rectangle recordingRectangle;
        private readonly UITimer labelRenderTimer;
        private readonly Stopwatch recordingTimer;
        private RecordingButton recordingButton;
        private ImageButton closeButton;
        private Label timerLabel;
        private Label beforeStartLabel;

        /// <summary>
        /// Initializes recording view.
        /// </summary>
        public RecordingVideoPanel(IRecorder recorder, Rectangle recordingRectangle)
        {
            this.recorder = recorder;
            this.recordingRectangle = recordingRectangle;

            InitializeComponents();

            recordingTimer = new Stopwatch();
            labelRenderTimer = new UITimer
            {
                Interval = 0.01
            };
            labelRenderTimer.Elapsed += RecordingLabelTimer_Elapsed;
            labelRenderTimer.Start();

            this.UnLoad += RecordingVideoPanelUnLoad;
        }

        private void RecordingVideoPanelUnLoad(object sender, EventArgs e)
        {
            recorder?.Stop();
        }

        private void RecordingButton_Clicked(object sender, System.EventArgs e)
        {
            recordingButton.Enabled = false;
            if (recordingButton.IsRecording)
            {
                StopRecording();
                recordingButton.Enabled = true;
            }
            else
            {
                recordingTimer.Reset();
                StartWithDelay();
            }
        }

        private void StartWithDelay()
        {
            int seconds = 3;

            var beforeRecordTimer = new UITimer()
            {
                Interval = 1
            };
            beforeRecordTimer.Elapsed += (o, e) =>
            {
                if (seconds != 1)
                {
                    seconds--;
                    beforeStartLabel.Text = seconds.ToString();
                }
                else
                {
                    beforeRecordTimer.Stop();
                    recorder.Start(recordingRectangle.OffsetRectangle(1));

                    while (!recorder.IsRecording)
                    {
                    }

                    beforeStartLabel.Text = "0";
                    recordingTimer.Start();
                    recordingButton.Enabled = true;
                }
            };
            beforeRecordTimer.Start();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            recorder.Stop();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void StopRecording()
        {
            recordingTimer.Stop();
            recorder.Stop();

            UploadingManager.RunUploading(recorder.GetVideo());
        }

        private void RecordingLabelTimer_Elapsed(object sender, EventArgs e)
        {
            timerLabel.Text = recordingTimer.Elapsed.ToString(@"hh\:mm\:ss");
            timerLabel.Invalidate();
        }

        private void InitializeComponents()
        {
            timerLabel = new Label()
            {
                Text = TimeSpan.Zero.ToString()
            };

            beforeStartLabel = new Label()
            {
                TextColor = Colors.Red,
                Text = "3",
                Width = 10
            };

            recordingButton = new RecordingButton(40, 35);
            recordingButton.Clicked += RecordingButton_Clicked;

            var closeImage = Icons.Close;
            closeButton = new ImageButton(new Size(40, 35), closeImage, scaleImageSize: new Size(20, 18));
            closeButton.Clicked += CloseButton_Clicked;

            Size = new Size(240, 40);
            Content = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 3,
                Size = new Size(240, 40),
                Spacing = 5,
                Items =
                {
                    recordingButton,
                    closeButton,
                    FormsHelper.GetVoidBox(15),
                    timerLabel,
                    FormsHelper.GetVoidBox(25),
                    beforeStartLabel
                }
            };
        }
    }
}
