using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace RedShot.Infrastructure.DomainServices.Services
{
    internal class LastFileService : ILastFileService
    {
        private readonly ReplaySubject<File> lastFileSubject;

        public LastFileService()
        {
            lastFileSubject = new ReplaySubject<File>(1);
        }

        public IObservable<File> LastFileNotification => lastFileSubject;

        public void SetLastFile(File file)
        {
            lastFileSubject.OnNext(file);
        }
    }
}
