using System.Collections.Generic;

namespace RedShot.Infrastructure.Abstractions.Recording
{
    /// <summary>
    /// Provides video and audio devices.
    /// </summary>
    public interface IRecordingDevices
    {
        /// <summary>
        /// Video devices.
        /// </summary>
        IEnumerable<IRecordingDevice> VideoDevices { get; }

        /// <summary>
        /// Audio devices.
        /// </summary>
        IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}