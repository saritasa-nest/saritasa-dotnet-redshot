using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface ILastFileService
    {
        IObservable<File> LastFileNotification { get; }

        void SetLastFile(File file);
    }
}
