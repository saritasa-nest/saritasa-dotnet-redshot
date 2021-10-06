using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Ftp
{
    public interface IFtpClient : IFileUploader, IDisposable
    {
        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
    }
}
