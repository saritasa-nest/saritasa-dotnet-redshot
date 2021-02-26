using RedShot.Infrastructure.Recording.Common.Devices;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Recording service.
    /// </summary>
    public interface IRecordingService
    {
        /// <summary>
        /// Get recorder.
        /// </summary>
        IRecorder GetRecorder();

        /// <summary>
        /// Install FFmpeg binaries.
        /// </summary>
        void InstallFFmpeg();

        /// <summary>
        /// Check on existing FFmpeg binaries.
        /// </summary>
        bool CheckFFmpeg();

        /// <summary>
        /// Get recording devices (Audio, Video).
        /// </summary>
        RecordingDevices GetRecordingDevices();
    }
}
