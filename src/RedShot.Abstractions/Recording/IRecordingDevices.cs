using System.Collections.Generic;

namespace RedShot.Abstractions.Recording
{
    public interface IRecordingDevices
    {
        IEnumerable<IRecordingDevice> VideoDevices { get; }

        IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}