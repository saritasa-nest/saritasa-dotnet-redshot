using Eto.Drawing;
using Eto.Forms;
using RedShot.Eto.Desktop.Forms.Tray;
using RedShot.Eto.Desktop.Resources;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RedShot.Eto.Desktop.Infrastructure
{
    internal class EtoNotificationService : INotificationService
    {
        public const string FailedNotificationName = "FailedNotification";
        public const string SucceedNotificationName = "SucceedNotification";

        public static ApplicationTray ApplicationTray { get; set; }

        public void Notify(string message, string title, NotifyStatus status = NotifyStatus.Success)
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

            notifyer.Show(ApplicationTray.Tray);
        }

        private static Bitmap GetIconForStatus(NotifyStatus status) => status switch
        {
            NotifyStatus.Failed => Icons.Failed,
            _ => Icons.Success
        };

        private static string GetStyleForStatus(NotifyStatus status) => status switch
            {
                NotifyStatus.Failed => FailedNotificationName,
                _ => SucceedNotificationName
        };
    }
}
