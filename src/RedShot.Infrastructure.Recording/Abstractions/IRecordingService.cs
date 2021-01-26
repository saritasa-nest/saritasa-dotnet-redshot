using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Abstractions
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
