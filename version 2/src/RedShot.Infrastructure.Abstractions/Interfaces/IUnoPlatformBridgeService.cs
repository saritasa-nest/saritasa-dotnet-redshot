using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IUnoPlatformBridgeService
    {
        Task OpenConfigurationViewAsync();

        Task OpenScreenshotViewAsync(File screenshot);
    }
}
