using System.Collections.Generic;

namespace RedShot.Infrastructure.Configuration.Models.Recording
{
    /// <summary>
    /// Contains data about audio parameters.
    /// </summary>
    public class AudioData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioData()
        {
            Devices = new List<DeviceData>();
        }

        /// <summary>
        /// Record audio flag.
        /// </summary>
        public bool RecordAudio { get; set; }

        /// <summary>
        /// Record audio devices.
        /// </summary>
        public IList<DeviceData> Devices { get; set; }
    }
}
