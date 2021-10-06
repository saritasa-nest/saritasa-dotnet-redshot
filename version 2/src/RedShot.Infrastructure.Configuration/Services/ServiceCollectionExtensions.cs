using Microsoft.Extensions.DependencyInjection;
using RedShot.Infrastructure.Abstractions.Interfaces;

namespace RedShot.Infrastructure.Configuration.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            return services;
        }
    }
}
