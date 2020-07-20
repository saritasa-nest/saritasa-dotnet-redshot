using System;
using System.Reflection;
using Eto.Forms;
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
            Eto.Style.Add<Eto.WinForms.Forms.FormHandler>("mystyle", h =>
            {
                // Windows Forms.
                var prop = h.Control.GetType().GetProperty("FormBorderStyle");
                if (prop != null)
                {
                    prop.SetValue(h.Control, 0);
                }
                // WPF.
                prop = h.Control.GetType().GetProperty("WindowStyle");
                if (prop != null)
                {
                    prop.SetValue(h.Control, 0);
                }
            });
            app.Run(ApplicationManager.GetTrayApp());
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
