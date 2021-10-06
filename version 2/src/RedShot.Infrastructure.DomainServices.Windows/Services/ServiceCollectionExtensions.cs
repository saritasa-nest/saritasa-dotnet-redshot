using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces.Recording;
using RedShot.Infrastructure.Recording.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.DomainServices.Windows.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRecordingServices(this IServiceCollection services)
        {
            services.AddSingleton<IRecordingAppInstaller, WindowsRecordingAppInstaller>();
            services.AddSingleton<IRecordingFoldersService, WindowsRecordingFoldersService>();
            services.AddTransient<IRecordingDevicesService, WindowsRecordingDevicesService>();
            services.AddTransient<IRecordingService, WindowsRecordingService>();

            return services;
        }
    }
}
