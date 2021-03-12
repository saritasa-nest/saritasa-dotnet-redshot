using Eto.Drawing;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Provides recorder.
    /// </summary>
    public interface IRecorder
    {
        /// <summary>
        /// Start recording.
        /// </summary>
        /// <param name="area">Selected area.</param>
        void Start(Rectangle area);

        /// <summary>
        /// Stop recording.
        /// </summary>
        void Stop();

        /// <summary>
        /// Get recorded video.
        /// </summary>
        File GetVideo();

        /// <summary>
        /// State of the recorder (Recording or not).
        /// </summary>
        bool IsRecording { get; }
    }
}
