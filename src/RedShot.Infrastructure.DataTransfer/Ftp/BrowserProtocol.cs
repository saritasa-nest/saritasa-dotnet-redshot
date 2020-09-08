using System.ComponentModel;

namespace RedShot.Infrastructure.DataTransfer.Ftp
{
    public enum BrowserProtocol
    {
        [Description("http://")]
        http,
        [Description("https://")]
        https,
        [Description("ftp://")]
        ftp,
        [Description("ftps://")]
        ftps,
        [Description("file://")]
        file
    }
}
