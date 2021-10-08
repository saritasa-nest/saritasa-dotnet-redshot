using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public enum NotifyStatus
    {
        Failed,
        Success
    }

    public interface INotificationService
    {
        void Notify(string message, string title, NotifyStatus status = NotifyStatus.Success);
    }
}
