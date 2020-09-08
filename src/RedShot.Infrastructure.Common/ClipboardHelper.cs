using Eto.Forms;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Common
{
    public static class ClipboardHelper
    {
        public static void SetStringInClipboard(string value, string serviceName)
        {
            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = value;

            NotifyHelper.Notify("A string was saved in clipboard", serviceName, NotifyStatus.Success);
        }
    }
}
