using RedShot.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RedShot.Desktop.Views
{
    /// <summary>
    /// Base class for navigation page.
    /// </summary>
    public class NavigationPage : Page
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NavigationPage()
        {
            DataContextChanged += NavigationPageDataContextChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is BaseViewModel)
            {
                DataContext = e.Parameter;
            }
        }

        private async void NavigationPageDataContextChanged(Windows.UI.Xaml.DependencyObject sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            var vm = DataContext as BaseViewModel;
            if (vm == null)
            {
                return;
            }

            try
            {
                vm.IsBusy = true;
                await vm.LoadAsync();
            }
            finally
            {
                vm.IsBusy = false;
            }
        }
    }
}
