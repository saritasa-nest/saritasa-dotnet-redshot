using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Helpers.Ffmpeg.Devices
{
    public class RecordingDevices
    {
        public List<Device> VideoDevices { get; } = new List<Device>();

        public List<Device> AudioDevices { get; } = new List<Device>();
    }
}
