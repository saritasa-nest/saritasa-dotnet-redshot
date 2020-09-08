using Eto.Forms;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Common
{
    public static class ClipboardHelper
    {
        public static void SetStringInClipboard(string value, string title, string serviceName)
        {
            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = value;

            NotifyHelper.Notify(title, serviceName, NotifyStatus.Success);
        }
    }
}
