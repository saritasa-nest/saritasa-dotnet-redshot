using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Eto.Mvp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEtoMvpServices(this IServiceCollection services)
        {
            services.AddSingleton<IEtoFormsBridgeService, EtoFormsBridgeService>();
            return services;
        }
    }
}
