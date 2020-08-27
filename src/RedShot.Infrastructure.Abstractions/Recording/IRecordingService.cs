namespace RedShot.Infrastructure.Abstractions.Recording
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
        bool InstallFFmpeg();

        /// <summary>
        /// Check on existing FFmpeg binaries.
        /// </summary>
        bool CheckFFmpeg();

        /// <summary>
        /// Gives recording devices (Audio, Video).
        /// </summary>
        IRecordingDevices GetRecordingDevices();
    }
}
