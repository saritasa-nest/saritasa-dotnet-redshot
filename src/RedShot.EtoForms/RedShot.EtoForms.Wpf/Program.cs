using System;
using System.Windows.Threading;
using Eto.Forms;

namespace RedShot.EtoForms.Wpf
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Eto.Style.Add<Eto.Wpf.Forms.FormHandler>("mystyle", h => { h.Control.WindowStyle = System.Windows.WindowStyle.None; });
            new Application(Eto.Platforms.Wpf).Run(ApplicationManager.GetTrayApp());           
        }
    }
}
