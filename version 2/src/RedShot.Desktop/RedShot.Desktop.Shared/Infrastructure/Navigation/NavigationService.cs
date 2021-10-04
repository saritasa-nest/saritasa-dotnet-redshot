using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Desktop.Views;
using RedShot.Mvvm.ServiceAbstractions.Navigation;
using RedShot.Mvvm.Utils;
using RedShot.Mvvm.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RedShot.Desktop.Shared.Infrastructure.Navigation
{
    /// <summary>
    /// Navigation service implementing page navigation.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly FrameBasedNavigation<NavigationPage> frameNavigation;
        private readonly ViewModelFactory viewModelFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NavigationService(Frame frame,
            ViewModelFactory viewModelFactory,
            NavigationStack navigationStack)
        {
            frameNavigation = new FrameBasedNavigation<NavigationPage>(frame, navigationStack, false);
            this.viewModelFactory = viewModelFactory;
        }

        /// <inheritdoc />
        public void GoBack()
        {
            frameNavigation.Close();
        }

        /// <inheritdoc />
        public Task OpenAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel
        {
            var viewModel = viewModelFactory.Create<TViewModel>(parameters);
            return OpenAsync(viewModel);
        }

        /// <inheritdoc />
        public async Task OpenAsync(BaseViewModel viewModel)
        {
            await frameNavigation.Open(viewModel);
        }

        /// <inheritdoc />
        public Task<TResult> OpenAsync<TViewModel, TResult>(params object[] parameters) where TViewModel : BaseViewModel, IWithResult<TResult>
        {
            var viewModel = viewModelFactory.Create<TViewModel>(parameters);
            return OpenAsync<TViewModel, TResult>(viewModel);
        }

        /// <inheritdoc />
        public async Task<TResult> OpenAsync<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>
        {
            return await frameNavigation.Open<TViewModel, TResult>(viewModel);
        }

        /// <inheritdoc/>
        public async Task OpenAsFirstAsync(BaseViewModel viewModel)
        {
            await frameNavigation.OpenAsFirstAsync(viewModel);
        }

        /// <inheritdoc/>
        public async Task OpenAsFirstAsync<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel
        {
            var viewModel = viewModelFactory.Create<TViewModel>(parameters);
            await frameNavigation.OpenAsFirstAsync(viewModel);
        }
    }
}
