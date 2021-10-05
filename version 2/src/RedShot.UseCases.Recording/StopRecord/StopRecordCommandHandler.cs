using MediatR;
using RedShot.Infrastructure.Recording.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.UseCases.Recording.StopRecord
{
    internal class StopRecordCommandHandler : AsyncRequestHandler<StopRecordCommand>
    {
        private readonly IRecordingService recordingService;

        public StopRecordCommandHandler(IRecordingService recordingService)
        {
            this.recordingService = recordingService;
        }

        protected override async Task Handle(StopRecordCommand request, CancellationToken cancellationToken)
        {
            await recordingService.StopAsync();
        }
    }
}
