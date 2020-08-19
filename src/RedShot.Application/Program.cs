using System;
using System.Reflection;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.Infrastructure;
using RedShot.Infrastructure.Configuration;
#if _UNIX
using System.Runtime.InteropServices;
#endif

namespace RedShot.EtoForms.Wpf
{
    /// <summary>
    /// Main class.
    /// </summary>
    internal class MainClass
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        public static void Main(string[] args)
        {
            Logger.Debug("The app was started!");

            var app = new Application(Eto.Platform.Detect);
            app.UnhandledException += InstanceOnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;

            AddControl();
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.Mac.SKControlHandler());
            }
            else
            {
                Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.GTK.SKControlHandler());
            }
#else
            throw new NotImplementedException();
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

            Logger.Fatal(ex);

            Application.Instance.Quit();
        }
    }
}
