using RedShot.Eto.Mvp.Presenters;
using RedShot.Eto.Mvp.ServiceAbstractions;
using RedShot.Infrastructure.Abstractions.Interfaces;
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

        public async Task<byte[]> OpenScreenSelectionForm()
        {
            var screenshot = await etoNavigationService.OpenFormAsync<ScreenshotsPresenter, IScreenshotsView, byte[]>();

            if (screenshot == null)
            {
                throw new TaskCanceledException();
            }

            return screenshot;
        }
    }
}
