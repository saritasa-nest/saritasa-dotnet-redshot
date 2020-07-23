using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using RedShot.App;

namespace RedShot.EtoForms.Wpf
{
    /// <summary>
    /// Main class.
    /// </summary>
    internal class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application(Eto.Platform.Detect);
            app.UnhandledException += InstanceOnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;

            AddStyle();
            app.Run(ApplicationManager.GetTrayApp());
        }

        private static void AddStyle()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.GTK.SKControlHandler());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Eto.Platform.Detect.Add<ISKControl>(() => new Eto.Forms.Controls.SkiaSharp.WinForms.SKControlHandler());
            }
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
            Application.Instance.Quit();
        }
    }
}
