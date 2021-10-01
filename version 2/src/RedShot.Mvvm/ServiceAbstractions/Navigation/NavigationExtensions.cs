using RedShot.Mvvm.ViewModels;

namespace RedShot.Mvvm.ServiceAbstractions.Navigation
{
    /// <summary>
    /// Contains extension methods for navigation in the app.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Open new page and do not wait for it to be closed.
        /// </summary>
        /// <param name="viewModel">View model.</param>
        public static void Open(this INavigationService navigationService, BaseViewModel viewModel)
        {
            navigationService.OpenAsync(viewModel);
        }

        /// <summary>
        /// Open new page with parameters and do not wait for it to be closed.
        /// </summary>
        /// <typeparam name="TViewModel">View model type.</typeparam>
        /// <param name="parameters">View model constructor parameters.</param>
        public static void Open<TViewModel>(this INavigationService navigationService, params object[] parameters) where TViewModel : BaseViewModel
        {
            navigationService.OpenAsync<TViewModel>(parameters);
        }
    }
}
