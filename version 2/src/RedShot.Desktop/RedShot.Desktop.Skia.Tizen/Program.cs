using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace RedShot.Desktop.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new RedShot.Desktop.App(), args);
            host.Run();
        }
    }
}
