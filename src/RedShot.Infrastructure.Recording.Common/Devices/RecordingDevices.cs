using System.Collections.Generic;

namespace RedShot.Infrastructure.Recording.Common.Devices
{
    /// <summary>
    /// Recording devices.
    /// </summary>
    public class RecordingDevices
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public RecordingDevices()
        {
            VideoDevices = new List<Device>();
            AudioDevices = new List<Device>();
        }

        /// <summary>
        /// Video devices.
        /// </summary>
        public List<Device> VideoDevices { get; }

        /// <summary>
        /// Audio devices.
        /// </summary>
        public List<Device> AudioDevices { get; }
    }
}
