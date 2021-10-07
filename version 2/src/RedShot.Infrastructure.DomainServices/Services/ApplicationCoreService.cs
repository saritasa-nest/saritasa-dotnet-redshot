using Microsoft.Extensions.Options;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.DomainServices.Services
{
    internal class ApplicationCoreService : IApplicationCoreService
    {
        private readonly IEtoFormsBridgeService etoFormsBridgeService;
        private readonly IUnoPlatformBridgeService unoPlatformBridgeService;
        private readonly IApplicationStateService applicationStateService;
        private readonly ILastFileService lastFileService;
        private readonly ApplicationSettings applicationSettings;

        private File lastFile;

        public ApplicationCoreService(
            IEtoFormsBridgeService etoFormsBridgeService,
            IUnoPlatformBridgeService unoPlatformBridgeService,
            IApplicationStateService applicationStateService,
            ILastFileService lastFileService,
            IOptions<ApplicationSettings> applicationSettings)
        {
            this.etoFormsBridgeService = etoFormsBridgeService;
            this.unoPlatformBridgeService = unoPlatformBridgeService;
            this.applicationStateService = applicationStateService;
            this.lastFileService = lastFileService;
            this.applicationSettings = applicationSettings.Value;
        }

        public void CloseApplication()
        {
            applicationStateService.ShutdownApplication();
        }

        public async Task OpenConfigurationAsync()
        {
            await unoPlatformBridgeService.OpenConfigurationViewAsync();
        }

        public Task OpenLastFileAsync()
        {
            throw new NotImplementedException();
        }

        public async Task RecordVideoAsync()
        {
            var recordingArea = await etoFormsBridgeService.OpenRecordingAreaSelectionForm();
            var video = await etoFormsBridgeService.OpenRecordingForm(recordingArea);

            lastFile = video;
            lastFileService.SetLastFile(video);
        }

        public async Task SendFeedBackAsync()
        {
            var url = $"mailto:{applicationSettings.DeveloperEmail}";

            await Task.Run(() =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            });
        }

        public async Task TakeScreenshotAsync()
        {
            var screenshot = await etoFormsBridgeService.OpenScreenshotForm();

            lastFile = screenshot;
            lastFileService.SetLastFile(screenshot);

            await unoPlatformBridgeService.OpenScreenshotViewAsync(screenshot);
        }
    }
}
