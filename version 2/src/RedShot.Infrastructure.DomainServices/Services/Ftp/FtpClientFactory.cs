using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Abstractions.Interfaces.Ftp;
using RedShot.Infrastructure.Domain.Ftp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.DomainServices.Services.Ftp
{
    internal class FtpClientFactory : IFtpClientFactory
    {
        private readonly IFormatNameService formatService;

        public FtpClientFactory(IFormatNameService formatService)
        {
            this.formatService = formatService;
        }

        public IFtpClient GetFtpClient(FtpAccount ftpAccount) => ftpAccount.Protocol switch
        {
            FtpProtocol.FTP or FtpProtocol.FTPS => new FtpClient(formatService, ftpAccount),
            FtpProtocol.SFTP => new SftpClient(formatService, ftpAccount),
            _ => throw new ArgumentOutOfRangeException(nameof(ftpAccount.Protocol), $"Not expected direction value: {ftpAccount.Protocol}"),
        };

    }
}
