using Eto.Drawing;
using RedShot.Upload.Abstractions;
using System;

namespace RedShot.Upload.Uploaders.FTP
{
    class FtpUploaderService : IUploaderService
    {
        public string ServiceIdentifier => "FTP/SFTP/FTPS";

        public string ServiceName => "FTP/SFTP/FTPS";

        public Icon ServiceIcon { get; }

        public Image ServiceImage { get; }

        public bool CheckConfig(IUploaderConfig config)
        {
            throw new NotImplementedException();
        }

        public IUploader CreateUploader()
        {
            throw new NotImplementedException();
        }
    }
}
