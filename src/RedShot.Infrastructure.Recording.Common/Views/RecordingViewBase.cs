using System;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Uploading;
using RedShot.Infrastructure.Uploading.Forms;

namespace RedShot.Infrastructure.Recording.Common.Views
{
    /// <summary>
    /// Recording view base.
    /// </summary>
    public partial class RecordingViewBase : Form
    {
        private UploadingForm uploadingForm;

        /// <summary>
        /// Recording service.
        /// </summary>
        protected readonly IRecordingService recordingService;

        /// <summary>
        /// Recording rectangle.
        /// </summary>
        protected readonly Rectangle recordingRectangle;

        /// <summary>
        /// Befor record timer.
        /// </summary>
        protected UITimer beforeRecordTimer;

        /// <summary>
        /// Rendering timer.
        /// </summary>
        protected UITimer renderingTimer;

        /// <summary>
        /// Recording timer.
        /// </summary>
        protected Stopwatch recordingTimer;

        /// <summary>
        /// Recording timer.
        /// </summary>
        protected RecordingButton recordingButton;

        /// <summary>
        /// Close button.
        /// </summary>
        protected ImageButton closeButton;

        /// <summary>
        /// Options button.
        /// </summary>
        protected ImageButton optionsButton;

        /// <summary>
        /// Timer label.
        /// </summary>
        protected Label timerLabel;

        /// <summary>
        /// Recorder.
        /// </summary>
        protected IRecorder recorder;

        /// <summary>
        /// Options panel.
        /// </summary>
        protected Control optionsPanel;

        /// <summary>
        /// Initializes recording view.
        /// </summary>
        public RecordingViewBase(IRecordingService recordingService, Rectangle recordingRectangle)
        {
            this.recordingService = recordingService;
            this.recordingRectangle = recordingRectangle;
            Topmost = true;
            Resizable = false;

            InitializeComponents();
        }

        /// <inheritdoc/>
        protected override void OnClosed(EventArgs e)
        {
            recordingTimer.Stop();
            renderingTimer.Stop();
            beforeRecordTimer?.Stop();
            recorder?.Stop();
            base.OnClosed(e);
        }

        /// <summary>
        /// Recording button clicked handler.
        /// </summary>
        protected void RecordingButtonClicked(object sender, System.EventArgs e)
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

        /// <summary>
        /// Start with delay.
        /// </summary>
        protected void StartWithDelay()
        {
            int seconds = 3;
            recordingButton.SetCountdownSecond(seconds);

            beforeRecordTimer = new UITimer()
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

        /// <summary>
        /// Finish recording.
        /// </summary>
        protected void FinishRecording()
        {
            recordingTimer.Stop();
            recorder.Stop();

            uploadingForm?.Close();
            uploadingForm = new UploadingForm(recorder.GetVideo(), UploadingProvider.GetUploadingServices());
            uploadingForm.Show();
        }

        /// <summary>
        /// Rendering timer elapsed handler.
        /// </summary>
        protected virtual void RenderingTimerElapsed(object sender, EventArgs e)
        {
            timerLabel.Text = recordingTimer.Elapsed.ToString(@"hh\:mm\:ss");
            timerLabel.Invalidate();
        }

        /// <summary>
        /// Options button clicked handler.
        /// </summary>
        protected void OptionsButtonClicked(object sender, EventArgs e)
        {
            using var optionsDialog = new AudioOptionsDialog(recordingService);
            optionsDialog.ShowModal();
        }
    }
}
