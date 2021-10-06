using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Abstractions.Interfaces.Ftp;
using RedShot.Infrastructure.DomainServices.Services.Ftp;
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
            services.AddSingleton<IEncryptionService, Base64Encrypter>();
            services.AddTransient<IFtpClientFactory, FtpClientFactory>();

            return services;
        }
    }
}
