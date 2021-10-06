using RedShot.Infrastructure.Domain.Ftp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Ftp
{
    public interface IFtpClientFactory
    {
        IFtpClient GetFtpClient(FtpAccount ftpAccount);
    }
}
