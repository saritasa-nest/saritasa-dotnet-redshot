using Microsoft.Extensions.Options;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Recording.Common
{
    /// <summary>
    /// Recording service.
    /// </summary>
    internal class WindowsRecordingService : IRecordingService
    {
        private readonly IOptionsMonitor<RecordingOptions> options;

        public WindowsRecordingService(IOptionsMonitor<RecordingOptions> options)
        {
            this.options = options;
        }

        public Task<IObservable<File>> StartAsync(Rectangle recordingArea)
        {

        }

        public Task StopAsync()
        {

        }
    }
}
