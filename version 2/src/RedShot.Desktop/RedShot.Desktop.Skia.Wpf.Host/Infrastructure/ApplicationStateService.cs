using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RedShot.Desktop.Skia.Wpf.Host.Infrastructure
{
    internal class ApplicationStateService : IApplicationStateService
    {
        private readonly Application application;

        public ApplicationStateService(Application application)
        {
            this.application = application;
        }

        public void ShutdownApplication()
        {
            application.Dispatcher.Invoke(() =>
            {
                application.Shutdown();
            });
        }
    }
}
