using MediatR;
using RedShot.Infrastructure.Abstractions.Interfaces.Ftp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.UseCases.UploadToFtp
{
    internal class UploadToFtpCommandHandler : IRequestHandler<UploadToFtpCommand>
    {
        private readonly IFtpClientFactory ftpClientFactory;

        public UploadToFtpCommandHandler(IFtpClientFactory ftpClientFactory)
        {
            this.ftpClientFactory = ftpClientFactory;
        }

        public async Task<Unit> Handle(UploadToFtpCommand request, CancellationToken cancellationToken)
        {
            using var client = ftpClientFactory.GetFtpClient(request.FtpAccount);
            var fileUrl = await client.UploadAsync(request.FileToUpload, cancellationToken);
        }
    }
}
