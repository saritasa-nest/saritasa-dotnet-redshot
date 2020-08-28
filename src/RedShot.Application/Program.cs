using System;
using System.Reflection;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Infrastructure;
using RedShot.Infrastructure.Configuration;
#if _WINDOWS
using Eto.WinForms.Forms;
#elif _UNIX
using System.Runtime.InteropServices;
#endif

namespace RedShot.Application
{
    /// <summary>
    /// Main class.
    /// </summary>
    internal class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Runs the app.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            logger.Debug("The app was started!");

            AppInitializer.Initialize();

            var app = new Eto.Forms.Application(Eto.Platform.Detect);
            app.UnhandledException += InstanceOnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;

            AddControl();
            AddStyles();
            app.Run(ApplicationManager.GetTrayApp());
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            ConfigurationManager.Save();
        }

        private static void AddControl()
        {
#if _WINDOWS
            Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.WinForms.SKControlHandler());
#elif _UNIX
            Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.GTK.SKControlHandler());
#else
            throw new NotImplementedException();
#endif
        }

        private static void AddStyles()
        {
#if _WINDOWS
            Eto.Style.Add<NotificationHandler>("FailedNotification", h => h.NotificationIcon = NotificationIcon.Error);
            Eto.Style.Add<NotificationHandler>("SucceedNotification", h => h.NotificationIcon = NotificationIcon.Info);
#endif
        }

        private static void InstanceOnUnhandledException(object sender, Eto.UnhandledExceptionEventArgs e)
        {
            ShowException(e.ExceptionObject as Exception);
        }

        private static void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowException(e.ExceptionObject as Exception);
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

            logger.Fatal(ex);

            Eto.Forms.Application.Instance.Quit();
        }
    }
}
