using System.Collections.Generic;

namespace RedShot.Infrastructure.Abstractions.Recording
{
    public interface IRecordingDevices
    {
        IEnumerable<IRecordingDevice> VideoDevices { get; }

        IEnumerable<IRecordingDevice> AudioDevices { get; }
    }
}