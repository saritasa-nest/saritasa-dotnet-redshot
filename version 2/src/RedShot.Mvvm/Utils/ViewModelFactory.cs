using Microsoft.Extensions.DependencyInjection;
using RedShot.Mvvm.ViewModels;
using System;

namespace RedShot.Mvvm.Utils
{
    /// <summary>
    /// Factory for creating view model instances.
    /// </summary>
    public class ViewModelFactory
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create a new instance of view model.
        /// </summary>
        /// <typeparam name="T">Type of the view model.</typeparam>
        /// <param name="viewModelparameters">Any constructor parameters to be passed to the view model.</param>
        /// <returns>Created view model.</returns>
        public T Create<T>(params object[] viewModelparameters) where T : BaseViewModel
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, viewModelparameters);
        }
    }
}
