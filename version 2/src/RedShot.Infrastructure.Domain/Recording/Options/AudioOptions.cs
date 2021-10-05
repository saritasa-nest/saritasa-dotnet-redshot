using RedShot.Infrastructure.Domain.Recording.Devices;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Domain.Recording.Options
{
    /// <summary>
    /// Audio options.
    /// </summary>
    public class AudioOptions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioOptions()
        {
            Devices = new HashSet<Device>();
        }

        /// <summary>
        /// Record audio flag.
        /// </summary>
        public bool RecordAudio { get; set; }

        /// <summary>
        /// Record audio devices.
        /// </summary>
        public HashSet<Device> Devices { get; set; }
    }
}
