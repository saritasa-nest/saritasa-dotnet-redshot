using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Mvvm.Infrastructure
{
    internal class UnoPlatformBridgeService : IUnoPlatformBridgeService
    {
        public Task OpenConfigurationViewAsync()
        {
            return Task.CompletedTask;
        }

        public Task OpenScreenshotViewAsync(File screenshot)
        {
            return Task.CompletedTask;
        }
    }
}
