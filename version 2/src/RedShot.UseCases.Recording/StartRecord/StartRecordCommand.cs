using MediatR;
using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RedShot.UseCases.Recording.StartRecord
{
    public class StartRecordCommand : IRequest<IObservable<File>>
    {
        public Rectangle Area { get; set; }
    }
}
