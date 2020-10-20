using RedShot.Infrastructure.Abstractions.Recording;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Recording.Ffmpeg.Devices
{
    /// <summary>
    /// Recording devices.
    /// </summary>
    public class RecordingDevices : IRecordingDevices
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public RecordingDevices(IEnumerable<IRecordingDevice> videoDevices = null, IEnumerable<IRecordingDevice> audioDevices = null)
        {
            VideoDevices = videoDevices ?? new List<IRecordingDevice>();
            AudioDevices = audioDevices ?? new List<IRecordingDevice>();
        }

        /// <summary>
        /// Video devices.
        /// </summary>
        public IEnumerable<IRecordingDevice> VideoDevices { get; }

        /// <summary>
        /// Audio devices.
        /// </summary>
        public IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}
