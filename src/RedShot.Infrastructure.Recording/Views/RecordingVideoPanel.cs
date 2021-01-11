using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Abstractions;
using RedShot.Infrastructure.Uploading;
using RedShot.Resources;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Recording video panel.
    /// </summary>
    internal class RecordingVideoPanel : Panel
    {
        /// <summary>
        /// Closed event.
        /// </summary>
        public event EventHandler Closed;

        private readonly IRecordingService recordingService;
        private IRecorder recorder;
        private Rectangle recordingRectangle;
        private readonly UITimer labelRenderTimer;
        private readonly Stopwatch recordingTimer;
        private RecordingButton recordingButton;
        private ImageButton closeButton;
        private ImageButton optionsButton;
        private Label timerLabel;

        /// <summary>
        /// Initializes recording view.
        /// </summary>
        public RecordingVideoPanel(IRecordingService recordingService, Rectangle recordingRectangle)
        {
            this.recordingService = recordingService;
            this.recordingRectangle = recordingRectangle;

            InitializeComponents();

            recordingTimer = new Stopwatch();
            labelRenderTimer = new UITimer
            {
                Interval = 0.01
            };
            labelRenderTimer.Elapsed += RecordingLabelTimerElapsed;
            labelRenderTimer.Start();

            this.UnLoad += RecordingVideoPanelUnLoad;
        }

        private void RecordingVideoPanelUnLoad(object sender, EventArgs e)
        {
            recorder?.Stop();
        }

        private void RecordingButtonClicked(object sender, System.EventArgs e)
        {
            recordingButton.Enabled = false;
            if (recordingButton.IsRecording)
            {
                FinishRecording();
                recordingButton.RevertState();
                recordingButton.Enabled = true;
                optionsButton.Enabled = true;
            }
            else
            {
                optionsButton.Enabled = false;
                recordingTimer.Reset();
                recorder = recordingService.GetRecorder();
                StartWithDelay();
            }
        }

        private void StartWithDelay()
        {
            int seconds = 3;
            recordingButton.SetCountdownSecond(seconds);

            var beforeRecordTimer = new UITimer()
            {
                Interval = 1
            };
            beforeRecordTimer.Elapsed += (o, e) =>
            {
                if (seconds != 1)
                {
                    seconds--;
                    recordingButton.SetCountdownSecond(seconds);
                }
                else
                {
                    beforeRecordTimer.Stop();
                    recorder.Start(recordingRectangle.OffsetRectangle(1));

                    while (!recorder.IsRecording)
                    {
                    }

                    recordingButton.SetCountdownSecond(0);
                    recordingButton.RevertState();
                    recordingTimer.Start();
                    recordingButton.Enabled = true;
                }
            };
            beforeRecordTimer.Start();
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            recorder?.Stop();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void FinishRecording()
        {
            recordingTimer.Stop();
            recorder.Stop();

            UploadingManager.RunUploading(recorder.GetVideo());
        }

        private void RecordingLabelTimerElapsed(object sender, EventArgs e)
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
            var buttonSize = new Size(40, 35);
            var scaleSize = new Size(20, 18);

            recordingButton = new RecordingButton(buttonSize);
            recordingButton.Clicked += RecordingButtonClicked;

            closeButton = new ImageButton(buttonSize, Icons.Close, scaleImageSize: scaleSize);
            closeButton.ToolTip = "Close";
            closeButton.Clicked += CloseButtonClicked;

            optionsButton = new ImageButton(buttonSize, Icons.Gear, scaleImageSize: scaleSize);
            optionsButton.ToolTip = "Audio Options";
            optionsButton.Clicked += OptionsButtonClicked;

            Size = new Size(220, 41);
            Content = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = 3,
                Spacing = 5,
                Items =
                {
                    recordingButton,
                    timerLabel,
                    FormsHelper.GetVoidBox(15),
                    optionsButton,
                    closeButton
                }
            };
        }

        private void OptionsButtonClicked(object sender, EventArgs e)
        {
            using var optionsDialog = new AudioOptionsDialog();
            optionsDialog.ShowModal();
        }
    }
}
