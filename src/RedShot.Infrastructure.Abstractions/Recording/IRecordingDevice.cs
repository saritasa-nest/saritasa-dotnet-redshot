namespace RedShot.Infrastructure.Abstractions.Recording
{
    /// <summary>
    /// Recording device.
    /// </summary>
    public interface IRecordingDevice
    {
        /// <summary>
        /// Name of the device.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Compatible with FFmpeg name of the device.
        /// </summary>
        string CompatibleFfmpegName { get; }
    }
}