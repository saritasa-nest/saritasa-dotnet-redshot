using RedShot.Desktop.Infrastructure.Common.Navigation;
using RedShot.Mvvm.ServiceAbstractions.Navigation;
using RedShot.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RedShot.Desktop.Shared.Infrastructure.Navigation
{
    /// <summary>
    /// Implements a navigation using a single frame for displaying the current view.
    /// </summary>
    /// <typeparam name="T">
    /// Type of view that are allowed to be navigated.
    /// Only views that have a <see cref="UsesViewModelAttribute"/> attribute specified are allowed to be uesd with this service.
    /// </typeparam>
    internal class FrameBasedNavigation<T>
    {
        private readonly Frame frame;
        private readonly Dictionary<Type, Type> viewModelToViewAssociation;
        private readonly NavigationStack navigationStack;

        private TaskCompletionSource<T> navigatingFinishTaskCompletionSource;

        /// <summary>
        /// Auto hide.
        /// </summary>
        public bool AutoHide { get; }

        /// <summary>
        /// Initializes a new frame based navigation service.
        /// </summary>
        /// <param name="frame">Frame control.</param>
        /// <param name="navigationStack">Navigation stack.</param>
        /// <param name="autoHide">Auto hide flag.</param>
        public FrameBasedNavigation(Frame frame, NavigationStack navigationStack, bool autoHide)
        {
            this.frame = frame;
            this.frame.Navigating += Frame_Navigating;
            this.frame.Navigated += Frame_Navigated;
            this.navigationStack = navigationStack;
            AutoHide = autoHide;

            // Find all existing pages and generate a viewmodel - page mapping
            viewModelToViewAssociation = new Dictionary<Type, Type>();
            var pageTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)));
            foreach (var pageType in pageTypes)
            {
                var viewModelAttribute = pageType.GetCustomAttribute<UsesViewModelAttribute>();
                if (viewModelAttribute == null)
                {
                    continue;
                }

                viewModelToViewAssociation.Add(viewModelAttribute.ViewModelType, pageType);
            }
        }

        /// <summary>
        /// Open the specified view model and close previous pages if any exists.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="viewModel">View model instance.</param>
        public async Task OpenAsFirstAsync<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel
        {
            ClearFrameHistory();
            await OpenInternal(viewModel);
        }

        private void ClearFrameHistory()
        {
            if (!frame.CanGoBack && !frame.CanGoForward)
            {
                return;
            }

            frame.BackStack?.Clear();
            frame.ForwardStack?.Clear();

            var model = navigationStack.Pop<T>();
            while (model != null)
            {
                model.NavigationResult.SetCanceled();
                model = navigationStack.Pop<T>();
            }
        }

        /// <summary>
        /// Open the specified view model.
        /// </summary>
        /// <param name="viewModel">View model instance.</param>
        public Task Open(BaseViewModel viewModel)
        {
            return OpenInternal(viewModel);
        }

        /// <summary>
        /// Open the specified view model and get a result from it.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <typeparam name="TResult">Type of the result to receive from it.</typeparam>
        /// <param name="viewModel">View model instance.</param>
        /// <returns>View model result.</returns>
        public Task<TResult> Open<TViewModel, TResult>(TViewModel viewModel) where TViewModel : BaseViewModel, IWithResult<TResult>
        {
            return OpenInternal(viewModel).ContinueWith(result => viewModel.Result);
        }

        private async Task OpenInternal(BaseViewModel viewModel)
        {
            var state = new ViewState(frame)
            {
                ViewModel = viewModel,
                NavigationResult = new TaskCompletionSource<object>(),
            };

            navigationStack.Push<T>(state);

            var viewType = GetViewType(viewModel.GetType());

            await WaitForNavigatingFinish();

            frame.Navigate(viewType, viewModel);
            await state.NavigationResult.Task;
        }

        /// <summary>
        /// Close the currently opened view.
        /// </summary>
        public void Close()
        {
            var currentViewModel = navigationStack.Pop<T>();
            currentViewModel.NavigationResult.SetResult(default);
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }

            var nextView = navigationStack.Peek<T>();
            nextView.EnsureFrameVisibility();
            currentViewModel.ViewModel.Dispose();
        }

        private void Frame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            navigatingFinishTaskCompletionSource?.TrySetCanceled();
            navigatingFinishTaskCompletionSource = new TaskCompletionSource<T>();
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            navigatingFinishTaskCompletionSource?.TrySetResult(default);

            var currentView = navigationStack.Peek<T>();
            currentView.EnsureFrameVisibility();
            frame.Focus(FocusState.Programmatic);
        }

        private Task WaitForNavigatingFinish()
        {
            if (navigatingFinishTaskCompletionSource != null)
            {
                return navigatingFinishTaskCompletionSource.Task;
            }

            return Task.CompletedTask;
        }

        private Type GetViewType(Type viewModelType)
        {
            if (!viewModelToViewAssociation.TryGetValue(viewModelType, out var viewType))
            {
                throw new ArgumentException($"Cannot find a view type for the {viewModelType} type.");
            }

            return viewType;
        }
    }
}
