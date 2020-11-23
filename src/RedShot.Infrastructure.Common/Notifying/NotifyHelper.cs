using System.Linq;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions;
using RedShot.Resources;

namespace RedShot.Infrastructure.Common.Notifying
{
    /// <summary>
    /// Notify helper.
    /// </summary>
    public static class NotifyHelper
    {
        private static TrayIndicator tray = GetTrayIndicator();

        /// <summary>
        /// Runs notifier with specified data.
        /// </summary>
        public static void Notify(string message, string title, NotifyStatus status = NotifyStatus.Success)
        {
            var notifyer = new Notification()
            {
                Message = message,
                Title = title
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                notifyer.Style = GetStyleForStatus(status);
            }
            else
            {
                var icon = new Bitmap(GetIconForStatus(status), 50, 50, ImageInterpolation.High);
                notifyer.ContentImage = icon;
            }

            notifyer.Show(tray);
        }

        private static TrayIndicator GetTrayIndicator()
        {
            var form = (ITrayForm)Application.Instance.Windows.First(w => w is ITrayForm);
            return form.Tray;
        }

        private static Bitmap GetIconForStatus(NotifyStatus status)
        {
            return status switch
            {
                NotifyStatus.Failed => Icons.Failed,
                _ => Icons.Success
            };
        }

        private static string GetStyleForStatus(NotifyStatus status)
        {
            return status switch
            {
                NotifyStatus.Failed => "FailedNotification",
                _ => "SucceedNotification"
            };
        }
    }
}
