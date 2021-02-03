using System;
using System.Linq;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Abstractions;
using RedShot.Infrastructure.Uploading;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Recording view.
    /// </summary>
    internal partial class RecordingView : Form
    {
        private readonly IRecordingService recordingService;
        private readonly Rectangle recordingRectangle;
        private UITimer beforeRecordTimer;
        private UITimer renderingTimer;
        private Stopwatch recordingTimer;
        private RecordingButton recordingButton;
        private ImageButton closeButton;
        private ImageButton optionsButton;
        private Label timerLabel;
        private IRecorder recorder;
        private Control optionsPanel;
        private Rectangle[] excludeRectangles;
        private bool isOptionsPanelHidden;

        /// <summary>
        /// Initializes recording view.
        /// </summary>
        public RecordingView(IRecordingService recordingService, Rectangle recordingRectangle)
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

        private void FinishRecording()
        {
            recordingTimer.Stop();
            recorder.Stop();

            UploadingManager.RunUploading(recorder.GetVideo());
        }

        private void RenderingTimerElapsed(object sender, EventArgs e)
        {
            timerLabel.Text = recordingTimer.Elapsed.ToString(@"hh\:mm\:ss");
            timerLabel.Invalidate();

#if _WINDOWS
            var optionsPanelRectangle = new Rectangle(optionsPanel.Size);
            if (!CheckPanelFit())
            {
                optionsPanelRectangle.Location = new Point(1, 1);
            }

            if (recorder != null && recorder.IsRecording)
            {
                var rectangle = new Rectangle(Location, optionsPanel.Size);
                var mouseRectangle = new Rectangle((Point)Mouse.Position, new Size(1, 1));
                if (!rectangle.Intersects(mouseRectangle))
                {
                    if (!isOptionsPanelHidden)
                    {
                        Platforms.Windows.WindowsRegionHelper.Exclude(ControlObject, excludeRectangles.Append(optionsPanelRectangle).ToArray());
                        isOptionsPanelHidden = true;
                    }
                    return;
                }
            }

            if (isOptionsPanelHidden)
            {
                Platforms.Windows.WindowsRegionHelper.Union(ControlObject, optionsPanelRectangle);
                Platforms.Windows.WindowsRegionHelper.Exclude(ControlObject, excludeRectangles);
                isOptionsPanelHidden = false;
            }
#endif
        }

        private bool CheckPanelFit()
        {
            return recordingRectangle.Location.Y - optionsPanel.Size.Height > 0;
        }

        private void OptionsButtonClicked(object sender, EventArgs e)
        {
            using var optionsDialog = new AudioOptionsDialog();
            optionsDialog.ShowModal();
        }
    }
}
