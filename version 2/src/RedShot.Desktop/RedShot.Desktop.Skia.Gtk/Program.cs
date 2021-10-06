using GLib;
using System;
using Uno.UI.Runtime.Skia;

namespace RedShot.Desktop.Skia.Gtk
{
    class Program
    {
        static void Main(string[] args)
        {
            ExceptionManager.UnhandledException += delegate (UnhandledExceptionArgs expArgs)
            {
                Console.WriteLine("GLIB UNHANDLED EXCEPTION" + expArgs.ExceptionObject.ToString());
                expArgs.ExitApplication = true;
            };

            var compositionRoot = new CompositionRoot();
            var host = new GtkHost(() => new App(compositionRoot), args);

            host.Run();
        }
    }
}
