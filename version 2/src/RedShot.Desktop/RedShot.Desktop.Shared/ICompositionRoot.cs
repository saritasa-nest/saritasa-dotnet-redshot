using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedShot.Desktop
{
    public interface ICompositionRoot : IDisposable
    {
        /// <summary>
        /// Service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Preparing DI.
        /// </summary>
        void ConfigureServices();

        /// <summary>
        /// Run the app and show the main page.
        /// </summary>
        Task RunAsync();
    }
}
