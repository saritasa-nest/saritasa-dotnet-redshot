using Eto.Drawing;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Recording.Common.Views;

namespace RedShot.Infrastructure.Recording.Views
{
    /// <summary>
    /// Unix recording view.
    /// </summary>
    public partial class RecordingView : RecordingViewBase
    {
        /// <summary>
        /// Constructor..
        /// </summary>
        public RecordingView(IRecordingService recordingService, Rectangle recordingRectangle) : base (recordingService, recordingRectangle)
        {
        }
    }
}
