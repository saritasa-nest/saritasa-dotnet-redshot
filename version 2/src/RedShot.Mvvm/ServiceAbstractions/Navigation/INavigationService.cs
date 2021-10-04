using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Mvvm.ViewModels;
using System.Threading.Tasks;

namespace RedShot.Mvvm.ServiceAbstractions.Navigation
{
    /// <summary>
    /// Service for navigation between pages in the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Open new page and close previous pages if any exists.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        Task OpenAsFirstAsync(BaseViewModel viewModel);

        /// <summary>
        /// Open new page and close previous pages if any exists.
        /// </summary>
        /// <typeparam name="TViewModel">View model type.</typeparam>
        /// <param name="parameters">View model constructor parameters.</param>
        Task OpenAsFirstAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel;

        /// <summary>
        /// Open new page with parameters.
        /// </summary>
        /// <typeparam name="TViewModel">View model type.</typeparam>
        /// <param name="parameters">View model constructor parameters.</param>
        /// <returns>Page result.</returns>
        Task OpenAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel;

        /// <summary>
        /// Open a new page for specific viewmodel.
        /// </summary>
        /// <param name="viewModel">View model instance.</param>
        Task OpenAsync(BaseViewModel viewModel);

        /// <summary>
        /// Open a page and get the result from it.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model.</typeparam>
        /// <typeparam name="TResult">Type of result to receive.</typeparam>
        /// <param name="parameters">View model constructor parameters.</param>
        /// <returns>Page result.</returns>
        Task<TResult> OpenAsync<TViewModel, TResult>(params object[] parameters) where TViewModel : BaseViewModel, IWithResult<TResult>;

        /// <summary>
        /// Open new page.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        /// <returns>Page result.</returns>
        Task<TResult> OpenAsync<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>;

        /// <summary>
        /// Close the current page and open the previous one.
        /// </summary>
        void GoBack();
    }
}
