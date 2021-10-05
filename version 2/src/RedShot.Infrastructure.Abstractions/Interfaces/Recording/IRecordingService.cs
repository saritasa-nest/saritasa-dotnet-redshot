using RedShot.Infrastructure.Domain.Files;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Recording service.
    /// </summary>
    public interface IRecordingService
    {
        Task<IObservable<File>> StartAsync(Rectangle recordingArea);

        Task StopAsync();
    }
}
