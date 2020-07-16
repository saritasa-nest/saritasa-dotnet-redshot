using System;
using Eto.Forms;

namespace RedShot.EtoForms.Gtk
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Eto.Style.Add<Eto.GtkSharp.Forms.FormHandler>("mystyle", h => { h.WindowStyle = WindowStyle.None; });
            new Application(Eto.Platforms.Gtk).Run();
        }
    }
}
