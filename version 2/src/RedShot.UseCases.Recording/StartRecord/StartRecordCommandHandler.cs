using MediatR;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Recording.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.UseCases.Recording.StartRecord
{
    internal class StartRecordCommandHandler : IRequestHandler<StartRecordCommand, IObservable<File>>
    {
        private readonly IRecordingService recordingService;

        public StartRecordCommandHandler(IRecordingService recordingService)
        {
            this.recordingService = recordingService;
        }

        public async Task<IObservable<File>> Handle(StartRecordCommand request, CancellationToken cancellationToken)
        {
            return await recordingService.StartAsync(request.Area);
        }
    }
}
