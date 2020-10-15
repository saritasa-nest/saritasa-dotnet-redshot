using RedShot.Infrastructure.Abstractions.Recording;
using System.Collections.Generic;

namespace RedShot.Infrastructure.Recording.Ffmpeg.Devices
{
    public class RecordingDevices : IRecordingDevices
    {
        public RecordingDevices(IEnumerable<IRecordingDevice> videoDevices = null, IEnumerable<IRecordingDevice> audioDevices = null)
        {
            VideoDevices = videoDevices ?? new List<IRecordingDevice>();
            AudioDevices = audioDevices ?? new List<IRecordingDevice>();
        }

        public IEnumerable<IRecordingDevice> VideoDevices { get; }

        public IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}
