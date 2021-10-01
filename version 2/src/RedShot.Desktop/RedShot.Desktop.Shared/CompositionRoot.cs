using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedShot.Desktop.Shared.Infrastructure;
using RedShot.Desktop.Shared.Infrastructure.Navigation;
using RedShot.Mvvm.ServiceAbstractions;
using RedShot.Mvvm.ServiceAbstractions.Navigation;
using RedShot.Mvvm.Utils;
using RedShot.Mvvm.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RedShot.Desktop
{
    internal class CompositionRoot : IDisposable
    {
        private ServiceProvider serviceProvider;

        /// <summary>
        /// Service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => serviceProvider;

        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        private static CompositionRoot instance;
        private bool disposedValue;

        /// <summary>
        /// Get an instance of this class.
        /// </summary>
        /// <returns></returns>
        public static CompositionRoot GetInstance()
        {
            if (instance == null)
            {
                instance = new CompositionRoot();
                instance.Configure();
            }
            return instance;
        }

        /// <summary>
        /// Preparing DI.
        /// </summary>
        private void Configure()
        {
            var builder = new ConfigurationBuilder();

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Run the app and show the main page.
        /// </summary>
        public async Task RunAsync()
        {
            try
            {
                var uiContext = ServiceProvider.GetRequiredService<IUiContext>();
                await uiContext.SwitchToUi();

                var navigation = ServiceProvider.GetRequiredService<INavigationService>();
                navigation.Open<MainMenuViewModel>();
            }
            catch (Exception exception)
            {
                var logger = ServiceProvider.GetRequiredService<ILogger<CompositionRoot>>();
                logger.LogCritical(exception, "Unexpected error occurred.");
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUiContext, UiContext>(provider => new UiContext(Window.Current.Dispatcher));
            services.AddSingleton<NavigationStack>();
            services.AddSingleton<ViewModelFactory>();
            services.AddSingleton<INavigationService, NavigationService>(provider => 
            {
                return new NavigationService(
                    Windows.UI.Xaml.Window.Current.Content as Frame,
                    provider.GetRequiredService<ViewModelFactory>(),
                    provider.GetRequiredService<NavigationStack>());
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    serviceProvider.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CompositionRoot()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
