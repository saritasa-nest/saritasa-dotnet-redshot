using RedShot.Infrastructure.Domain.Recording.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Recording
{
    public interface IRecordingDevicesService
    {
        Task<RecordingDevices> GetRecordingDevicesAsync();
    }
}
