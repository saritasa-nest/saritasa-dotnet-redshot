using Microsoft.Extensions.DependencyInjection;
using RedShot.Desktop.Shared.Infrastructure.Navigation;
using RedShot.Eto.Desktop;
using RedShot.Infrastructure.Configuration.Services;
using RedShot.Infrastructure.DomainServices.Services;
using RedShot.Mvvm.Infrastructure;
using RedShot.Mvvm.ServiceAbstractions.Navigation;
using RedShot.Mvvm.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace RedShot.Desktop.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<NavigationStack>();
            services.AddSingleton<ViewModelFactory>();
            services.AddSingleton<INavigationService, NavigationService>(provider =>
            {
                return new NavigationService(
                    Windows.UI.Xaml.Window.Current.Content as Frame,
                    provider.GetRequiredService<ViewModelFactory>(),
                    provider.GetRequiredService<NavigationStack>());
            });

            services.AddEtoServices();
            services.AddDomainServices();
            services.AddConfiguration();
            services.AddUnoServices();

            return services;
        }
    }
}
