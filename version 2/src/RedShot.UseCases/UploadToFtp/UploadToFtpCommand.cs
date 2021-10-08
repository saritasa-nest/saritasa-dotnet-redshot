using MediatR;
using RedShot.Infrastructure.Configuration.Models.Account;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Domain.Ftp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.UseCases.UploadToFtp
{
    public class UploadToFtpCommand : IRequest
    {
        public FtpAccount FtpAccount { get; set; }

        public File FileToUpload { get; set; }
    }
}
