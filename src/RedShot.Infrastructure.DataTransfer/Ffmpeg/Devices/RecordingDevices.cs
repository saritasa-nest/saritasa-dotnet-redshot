using RedShot.Infrastructure.Abstractions.Recording;
using System.Collections.Generic;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices
{
    public class RecordingDevices : IRecordingDevices
    {
        public RecordingDevices(IEnumerable<IRecordingDevice> videoDevices = null, IEnumerable<IRecordingDevice> audioDevices = null)
        {
            VideoDevices = videoDevices != null ? videoDevices : new List<IRecordingDevice>();
            AudioDevices = audioDevices != null ? audioDevices : new List<IRecordingDevice>();
        }

        public IEnumerable<IRecordingDevice> VideoDevices { get; }

        public IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}
