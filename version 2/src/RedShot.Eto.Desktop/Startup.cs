using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Eto;
using System.Threading;
using RedShot.Eto.Desktop.Forms.Tray;
using Eto.WinForms.Forms;
using Eto.Forms.Controls.SkiaSharp.WinForms;

using WinFormsPlatform = Eto.WinForms.Platform;
using System.Globalization;
using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Eto.Desktop.Infrastructure;
using RedShot.Eto.Mvp.ServiceAbstractions;
using RedShot.Eto.Mvp.Infrastructure;

#if _WINDOWS

#elif _UNIX
using System.Runtime.InteropServices;
#endif

namespace RedShot.Eto.Desktop
{
    public static class Startup
    {
        internal static TaskCompletionSource<Application> ApplicationInitializedCompletionSource { get; private set; }

        public static void RunEtoApplication(IServiceProvider provider)
        {
            ApplicationInitializedCompletionSource = new TaskCompletionSource<Application>();

            var thread = new Thread(() => StartApplication(provider));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public static IServiceCollection AddEtoServices(this IServiceCollection services)
        {
            services.AddSingleton<IEtoNavigationService, EtoNavigationService>();
            services.AddEtoMvpServices();

            return services;
        }

        [STAThread]
        private static void StartApplication(IServiceProvider provider)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            var app = new Application(Platform.Detect);
            app.UnhandledException += (o, e) => ShowException(e.ExceptionObject as Exception);
            //app.Initialized += AppInitialized;

            AddStyles();

            Platform.Detect.Add<ISKControl>(() => new SKControlHandler());


            var menuService = provider.GetRequiredService<IMenuService>();
            var tray = new ApplicationTray(menuService);
            tray.LoadComplete += (o, e) => ApplicationInitializedCompletionSource.SetResult(app);
            app.Run(tray);
        }

        private static void AddStyles()
        {
#if _WINDOWS
            Style.Add<NotificationHandler>("FailedNotification", h => h.NotificationIcon = NotificationIcon.Error);
            Style.Add<NotificationHandler>("SucceedNotification", h => h.NotificationIcon = NotificationIcon.Info);
#endif
        }

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Show exception in message box.
        /// </summary>
        /// <param name="ex">Exception to show.</param>
        private static void ShowException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            if (ex is TargetInvocationException tiex)
            {
                MessageBox.Show(tiex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxType.Error);
            }
            else
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxType.Error);
            }

            Application.Instance.Quit();
        }
    }
}
