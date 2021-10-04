using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.DomainServices.Services
{
    internal class MenuService : IMenuService
    {
        private readonly IEtoFormsBridgeService etoFormsBridgeService;

        public MenuService(IEtoFormsBridgeService etoFormsBridgeService)
        {
            this.etoFormsBridgeService = etoFormsBridgeService;
        }

        public async Task TakeScreenshotAsync()
        {
            var screenshot = await etoFormsBridgeService.OpenScreenSelectionForm();
        }
    }
}
