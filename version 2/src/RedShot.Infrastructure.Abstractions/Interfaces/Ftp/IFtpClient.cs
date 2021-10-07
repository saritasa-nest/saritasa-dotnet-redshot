using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces.Ftp
{
    public interface IFtpClient : IDisposable
    {
        Task<string> UploadAsync(File file, CancellationToken cancellationToken = default);

        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
    }
}
