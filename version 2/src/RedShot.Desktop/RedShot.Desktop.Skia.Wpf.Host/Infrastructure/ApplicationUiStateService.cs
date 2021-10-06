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
        private readonly Window mainWindow;
        private readonly Dispatcher dispatcher;

        public ApplicationUiStateService(Window mainWindow, Dispatcher dispatcher)
        {
            this.mainWindow = mainWindow;
            this.dispatcher = dispatcher;
        }

        public void HideUiInterface()
        {
            dispatcher.Invoke(() =>
            {
                mainWindow.Hide();
                mainWindow.ShowInTaskbar = false;
            }, DispatcherPriority.Loaded);
        }

        public void ShowUiInterface()
        {
            dispatcher.Invoke(() =>
            {
                mainWindow.ShowInTaskbar = true;
                mainWindow.Activate();
            });
        }
    }
}
