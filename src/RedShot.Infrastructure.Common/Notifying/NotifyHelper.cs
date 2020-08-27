using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;

namespace RedShot.Infrastructure.Common.Notifying
{
    /// <summary>
    /// Notify helper.
    /// </summary>
    public static class NotifyHelper
    {
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

            notifyer.Show();
        }

        private static Bitmap GetIconForStatus(NotifyStatus status)
        {
            return status switch
            {
                NotifyStatus.Failed => new Bitmap(Resources.Properties.Resources.Failed),
                _ => new Bitmap(Resources.Properties.Resources.Success)
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
