using System.ComponentModel;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models
{
    public enum BrowserProtocol
    {
        [Description("http://")]
        Http,
        [Description("https://")]
        Https,
        [Description("ftp://")]
        Ftp,
        [Description("ftps://")]
        Ftps,
        [Description("file://")]
        File
    }
}
