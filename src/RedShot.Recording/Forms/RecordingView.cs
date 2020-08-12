using Eto.Drawing;
using Eto.Forms;
using RedShot.Recording.Recorders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Recording.Forms
{
    public class RecordingView : Form
    {
        private Control managePanel;
        private Control capturePanel;
        private IRecorder recorder;

        public RecordingView(IRecordingManager recordingManager)
        {
            recorder = recordingManager.GetRecorder(new FFmpegOptions());

            MinimumSize = new Size(500, 500);
            this.Resizable = true;
            this.MovableByWindowBackground = true;

            InitializeComponents();
        }

        public RecordingView()
        {
            BackgroundColor = SystemColors.WindowBackground;
            MinimumSize = new Size(500, 500);
            this.Resizable = true;
            this.MovableByWindowBackground = true;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            managePanel = new StackLayout()
            {

            };

            capturePanel = new CapturePanel();

            managePanel.Size = new Size(500, 50);

            capturePanel.Size = new Size(500, 450);
            var region = new Region();

            this.


            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    managePanel,
                    capturePanel,
                }
            };
        }

        private void StartRecordingButtonClicked()
        {
            Rectangle rect = default;

            rect.Location = capturePanel.Location;
            rect.Size = capturePanel.Size;

            recorder.Start(rect);

            this.Resizable = false;
            this.MovableByWindowBackground = false;
        }

        private void SetRegion()
        {
            var prop = this.ControlObject.GetType().GetProperty("Region");
            if (prop != null)
            {
                prop.SetValue(h.Control, 0);
            }
        }
    }
}
