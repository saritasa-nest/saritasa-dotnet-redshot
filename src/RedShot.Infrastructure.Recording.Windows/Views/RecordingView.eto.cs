using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Recording.Common.Views;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Windows recording view.
    /// </summary>
    public partial class RecordingView : RecordingViewBase
    {
        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            InitializeOptionsPanel();

            Padding contentPadding = default;

            WindowStyle = WindowStyle.None;
            BackgroundColor = Colors.Red;

            Rectangle excludeRectangle = default;
            Rectangle excludeRectangle2 = default;

            if (CheckPanelFit())
            {
                Location = new Point(recordingRectangle.X, recordingRectangle.Y - optionsPanel.Height - 1);
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + optionsPanel.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, optionsPanel.Height), new Size(recordingRectangle.Width, recordingRectangle.Height + 1)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(optionsPanel.Width, 0), new Size(recordingRectangle.Width - optionsPanel.Width, optionsPanel.Height));
            }
            else
            {
                Location = recordingRectangle.Location;
                Size = new Size(recordingRectangle.Width, recordingRectangle.Height + 1);
                excludeRectangle = new Rectangle(new Point(0, optionsPanel.Height - 1), new Size(recordingRectangle.Width, recordingRectangle.Height - optionsPanel.Height + 2)).OffsetRectangle(1);
                excludeRectangle2 = new Rectangle(new Point(optionsPanel.Width + 1, 1), new Size(recordingRectangle.Width - optionsPanel.Width - 2, optionsPanel.Height));
                contentPadding = new Padding(1, 1, 0, 0);
            }

            excludeRectangles = new Rectangle[]
            {
                excludeRectangle,
                excludeRectangle2
            };
            Platforms.Windows.WindowsRegionHelper.Exclude(ControlObject, excludeRectangles);

            recordingTimer = new Stopwatch();
            renderingTimer = new UITimer
            {
                Interval = 0.01
            };
            renderingTimer.Elapsed += RenderingTimerElapsed;
            renderingTimer.Start();

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = contentPadding,
                Items =
                {
                    optionsPanel
                }
            };
        }
    }
}
