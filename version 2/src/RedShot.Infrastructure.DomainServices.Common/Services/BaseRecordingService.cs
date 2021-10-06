using Microsoft.Extensions.Options;
using RedShot.Infrastructure.Abstractions.Interfaces.Recording;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.DomainServices.Common.CliManagement;
using RedShot.Infrastructure.DomainServices.Common.Helpers;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using File = RedShot.Infrastructure.Domain.Files.File;

namespace RedShot.Infrastructure.DomainServices.Common.Services
{
    public abstract class BaseRecordingService : IRecordingService, IDisposable
    {
        private readonly IOptionsMonitor<RecordingOptions> optionsMonitor;
        private readonly IRecordingFoldersService foldersService;
        private readonly IRecordingAppInstaller recordingApplication;
        private readonly CliApplication cliApplication;
        private bool disposedValue;

        protected BaseRecordingService(
            IOptionsMonitor<RecordingOptions> optionsMonitor,
            IRecordingFoldersService foldersService,
            IRecordingAppInstaller recordingApplication)
        {
            this.optionsMonitor = optionsMonitor;
            this.foldersService = foldersService;
            this.recordingApplication = recordingApplication;

            cliApplication = new CliApplication();
        }

        public virtual async Task<IObservable<File>> StartAsync(Rectangle recordingArea)
        {
            var recordingOptions = optionsMonitor.CurrentValue;

            var videoName = $"RedShot-Video-{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";
            var videosFolder = foldersService.GetVideosFolder();
            var videoPath = Path.Combine(videosFolder, $"{videoName}.{recordingOptions.FFmpegOptions.Extension}");

            var startArguments = GenerateStartArguments(videoPath, recordingOptions, recordingArea);
            var fileName = await recordingApplication.GetRecorderFileNameAsync();
            await cliApplication.StartAsync(fileName, startArguments);

            var recordingObservable = new ReplaySubject<File>(1);
            cliApplication.ResponseMessages.Subscribe(
                onNext: (_) => { },
                onCompleted: () =>
                {
                    var file = new File(videoPath, FileType.Video);
                    recordingObservable.OnNext(file);
                    recordingObservable.OnCompleted();
                });

            return recordingObservable;
        }

        private string GenerateStartArguments(string videoPath, RecordingOptions recordingOptions, Rectangle recordingArea)
        {
            var deviceArguments = GenerateDeviceArguments(recordingOptions, recordingArea);

            var outputArguments = FfmpegArgumentsHelper.GenerateOutputArguments(videoPath);
            var videoArguments = FfmpegArgumentsHelper.GenerateVideoArguments(recordingOptions.FFmpegOptions);
            var audioArguments = FfmpegArgumentsHelper.GenerateAudioArguments(recordingOptions.AudioOptions);

            var startArgumentsBuilder = new StringBuilder();
            startArgumentsBuilder.Append("-thread_queue_size 1024 ");
            startArgumentsBuilder.Append($"{deviceArguments} ");
            startArgumentsBuilder.Append($"{videoArguments} ");
            startArgumentsBuilder.Append($"{audioArguments} ");
            startArgumentsBuilder.Append($"{outputArguments}");

            return startArgumentsBuilder.ToString();
        }

        public async Task StopAsync()
        {
            await cliApplication.InputCommandAsync("q");
            await cliApplication.CloseAsync();
        }

        protected abstract string GenerateDeviceArguments(RecordingOptions recordingOptions, Rectangle recordingArea);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cliApplication.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseRecordingService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
