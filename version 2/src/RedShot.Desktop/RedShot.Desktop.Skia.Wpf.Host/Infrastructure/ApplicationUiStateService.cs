using RedShot.Mvvm.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace RedShot.Desktop.Skia.Wpf.Host.Infrastructure
{
    internal class ApplicationUiStateService : IApplicationUiStateService
    {
        private readonly Application application;

        public ApplicationUiStateService(Application application)
        {
            this.application = application;
        }

        public void HideUiInterface()
        {
            application.Dispatcher.Invoke(() =>
            {
                application.MainWindow.Hide();
                application.MainWindow.ShowInTaskbar = false;
            }, DispatcherPriority.Loaded);
        }

        public void ShowUiInterface()
        {
            application.Dispatcher.Invoke(() =>
            {
                application.MainWindow.ShowInTaskbar = true;
                application.MainWindow.Activate();
            });
        }
    }
}
