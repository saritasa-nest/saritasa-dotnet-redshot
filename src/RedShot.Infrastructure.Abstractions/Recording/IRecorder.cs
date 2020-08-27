using Eto.Drawing;

namespace RedShot.Infrastructure.Abstractions.Recording
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
        /// Stops recording.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gives recorded video.
        /// </summary>
        IFile GetVideo();

        /// <summary>
        /// State of the recorder (Recording or not).
        /// </summary>
        bool IsRecording { get; }
    }
}
