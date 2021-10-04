using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.DomainServices.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddSingleton<IMenuService, MenuService>();

            return services;
        }
    }
}
