using RedShot.Eto.Mvp.Presenters;
using RedShot.Eto.Mvp.Presenters.Records;
using RedShot.Eto.Mvp.ServiceAbstractions;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Eto.Mvp.Infrastructure
{
    internal class EtoFormsBridgeService : IEtoFormsBridgeService
    {
        private readonly IEtoNavigationService etoNavigationService;

        public EtoFormsBridgeService(IEtoNavigationService etoNavigationService)
        {
            this.etoNavigationService = etoNavigationService;
        }

        public Task<File> OpenRecordingForm(Rectangle recordingArea)
        {
            throw new NotImplementedException();
        }

        public async Task<Rectangle> OpenRecordingAreaSelectionForm()
        {
            var result = await etoNavigationService.OpenFormAsync<RecordAreaSelectionPresenter, IRecordAreaSelectionView, SelectionResult>();

            if (result.Status == SelectionStatus.Canceled)
            {
                throw new TaskCanceledException();
            }

            if (result.Status == SelectionStatus.NeedToChangeScreen)
            {
                return await OpenRecordingAreaSelectionForm();
            }

            return result.SelectionArea;
        }

        public async Task<File> OpenScreenshotForm()
        {
            var result = await etoNavigationService.OpenFormAsync<ScreenshotsPresenter, IScreenshotsView, ScreenshotResult>();

            if (result.Status == ScreenshotStatus.Canceled)
            {
                throw new TaskCanceledException();
            }

            if (result.Status == ScreenshotStatus.NeedToChangeScreen)
            {
                return await OpenScreenshotForm();
            }

            return new File(result.Screenshot, FileType.Image);
        }
    }
}
