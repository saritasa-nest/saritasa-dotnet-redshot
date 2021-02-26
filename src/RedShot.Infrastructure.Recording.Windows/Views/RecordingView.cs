using System;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Recording.Common.Views;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Windows recording view.
    /// </summary>
    public partial class RecordingView : RecordingViewBase
    {
        private Rectangle[] excludeRectangles;
        private bool isOptionsPanelHidden;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordingView(IRecordingService recordingService, Rectangle recordingRectangle) : base (recordingService, recordingRectangle) 
        {
        }

        /// <inheritdoc/>
        protected override void RenderingTimerElapsed(object sender, EventArgs e)
        {
            base.RenderingTimerElapsed(sender, e);

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
        }

        private bool CheckPanelFit()
        {
            return recordingRectangle.Location.Y - optionsPanel.Size.Height > 0;
        }
    }
}
