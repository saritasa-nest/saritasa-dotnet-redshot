using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

namespace RedShot.Infrastructure.Recording.Abstractions
{
    /// <summary>
    /// Abstraction for recording services.
    /// Recorder factory.
    /// </summary>
    public interface IRecordingService
    {
        /// <summary>
        /// Gives recorder.
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
        /// Gives recording devices (Audio, Video).
        /// </summary>
        RecordingDevices GetRecordingDevices();
    }
}
