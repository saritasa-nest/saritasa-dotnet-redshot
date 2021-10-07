using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Mvvm.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnoServices(this IServiceCollection services)
        {
            services.AddSingleton<IUnoPlatformBridgeService, UnoPlatformBridgeService>();

            return services;
        }
    }
}
