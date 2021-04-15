using System;
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
        /// <param name="message">Message.</param>
        /// <param name="title">Title.</param>
        /// <param name="status">Notify status.</param>
        public static void Notify(string message, string title, NotifyStatus status = NotifyStatus.Success)
        {
            Notify(message, title, null, status);
        }

        /// <inheritdoc cref="Notify"/>
        /// <param name="onUserClick">Event handler on user click.</param>
        public static void Notify(string message, string title, Action onUserClick, NotifyStatus status = NotifyStatus.Success)
        {
            var notifier = new Notification()
            {
                Message = message,
                Title = title,
                ID = Guid.NewGuid().ToString()
            };

            if (onUserClick != null)
            {
                Application.Instance.NotificationActivated += (o, e) =>
                {
                    if (e.ID == notifier.ID)
                    {
                        onUserClick();
                    }
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                notifier.Style = GetStyleForStatus(status);
            }
            else
            {
                var icon = new Bitmap(GetIconForStatus(status), 50, 50, ImageInterpolation.High);
                notifier.ContentImage = icon;
            }

            notifier.Show(tray);
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
