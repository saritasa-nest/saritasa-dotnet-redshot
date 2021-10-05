using MediatR;
using RedShot.Infrastructure.Abstractions.Interfaces.Recording;
using RedShot.Infrastructure.Domain.Recording.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.UseCases.Recording.GetRecordingDevices
{
    internal class GetRecordingDevicesQueryHandler : IRequestHandler<GetRecordingDevicesQuery, RecordingDevices>
    {
        private readonly IRecordingDevicesService recordingService;

        public GetRecordingDevicesQueryHandler(IRecordingDevicesService recordingService)
        {
            this.recordingService = recordingService;
        }

        public async Task<RecordingDevices> Handle(GetRecordingDevicesQuery request, CancellationToken cancellationToken)
        {
            return await recordingService.GetRecordingDevicesAsync();
        }
    }
}
