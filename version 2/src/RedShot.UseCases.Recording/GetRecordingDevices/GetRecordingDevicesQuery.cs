using MediatR;
using RedShot.Infrastructure.Domain.Recording.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.UseCases.Recording.GetRecordingDevices
{
    public class GetRecordingDevicesQuery : IRequest<RecordingDevices>
    {
    }
}
