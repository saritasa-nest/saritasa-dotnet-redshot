using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Recording.Abstractions;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Recording view.
    /// </summary>
    internal class RecordingView : Form
    {
        private readonly IRecorder recorder;
        private readonly Rectangle recordingRectangle;

        /// <summary>
        /// Initializes recording view.
        /// </summary>
        public RecordingView(IRecorder recorder, Rectangle recordingRectangle)
        {
            this.recordingRectangle = recordingRectangle;
            this.recorder = recorder;
            Topmost = true;
            Resizable = false;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var recordingVideoPanel = new RecordingVideoPanel(recorder, recordingRectangle);
            recordingVideoPanel.Closed += RecordingVideoPanelClosed;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    recordingVideoPanel
                }
            };

#if _WINDOWS
            recordingVideoPanel.BackgroundColor = Colors.WhiteSmoke;
            BackgroundColor = Colors.Red;
            this.MovableByWindowBackground = false;

            WindowStyle = WindowStyle.None;
            BackgroundColor = Colors.Red;

            Rectangle excludeRectangle = default;
            Rectangle excludeRectangle2 = default;

            if ((recordingRectangle.Location.Y - recordingVideoPanel.Height) > 0)
            {
                Location = new Point(recordingRectangle.X, recordingRectangle.Y - recordingVideoPanel.Height - 1);
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + recordingVideoPanel.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, recordingVideoPanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(recordingVideoPanel.Width, 0), new Size(recordingRectangle.Width - recordingVideoPanel.Width, recordingVideoPanel.Height));
            }
            else
            {
                var screenBounds = ScreenHelper.GetScreenBounds();

                if ((recordingRectangle.Location.Y + recordingVideoPanel.Height) < screenBounds.Height)
                {
                    Location = recordingRectangle.Location;
                    Size = new Size(recordingRectangle.Width, recordingRectangle.Height + 1);
                    excludeRectangle = new Rectangle(new Point(0, recordingVideoPanel.Height - 1), new Size(recordingRectangle.Width, recordingRectangle.Height - recordingVideoPanel.Height + 2)).OffsetRectangle(1);
                    excludeRectangle2 = new Rectangle(new Point(recordingVideoPanel.Width, 1), new Size(recordingRectangle.Width - recordingVideoPanel.Width - 1, recordingVideoPanel.Height));
                }
            }

            RedShot.Platforms.Windows.WindowsRegionHelper.Exclude(this.ControlObject, excludeRectangle, excludeRectangle2);
#endif
        }

        private void RecordingVideoPanelClosed(object sender, EventArgs e)
        {
            Close();
        }
    }
}
