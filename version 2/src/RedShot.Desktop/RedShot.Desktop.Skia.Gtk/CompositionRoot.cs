using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedShot.Desktop.Infrastructure;
using RedShot.Eto.Desktop;
using RedShot.Mvvm.ServiceAbstractions;
using RedShot.Mvvm.ServiceAbstractions.Navigation;
using RedShot.Mvvm.ViewModels;
using Windows.UI.Xaml;

namespace RedShot.Desktop.Skia.Gtk
{
    internal class CompositionRoot : ICompositionRoot
    {
        private ServiceProvider serviceProvider;
        private bool disposedValue;

        /// <summary>
        /// Service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => serviceProvider;

        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Preparing DI.
        /// </summary>
        public void ConfigureServices()
        {
            var builder = new ConfigurationBuilder();
            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSharedServices();

            //var dispatcher = Application.Current.Dispatcher;
            //services.AddSingleton<IUiContext, UiContext>(provider => new UiContext(dispatcher));

            serviceProvider = services.BuildServiceProvider();
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

                Startup.RunEtoApplication(ServiceProvider);

                var navigation = ServiceProvider.GetRequiredService<INavigationService>();
                navigation.Open<MainMenuViewModel>();
            }
            catch (Exception exception)
            {
                var logger = ServiceProvider.GetRequiredService<ILogger<CompositionRoot>>();
                logger.LogCritical(exception, "Unexpected error occurred.");
                throw;
            }
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
