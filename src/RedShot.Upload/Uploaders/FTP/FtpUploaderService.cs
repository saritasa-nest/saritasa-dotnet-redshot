using Eto.Drawing;
using RedShot.Helpers.FtpModels;
using RedShot.Upload.Abstractions;
using RedShot.Upload.Basics;
using System;

namespace RedShot.Upload.Uploaders.FTP
{
    public class FtpUploaderService : IUploaderService
    {
        public FtpUploaderService(FtpAccount account)
        {
            Account = account;
        }
        public FtpAccount Account { get; }

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
            if (Account != null)
            {
                if (Account.Protocol == FtpProtocol.FTP || Account.Protocol == FtpProtocol.FTPS)
                {
                    return new Ftp(Account);
                }
                else if (Account.Protocol == FtpProtocol.SFTP)
                {
                    return new Sftp(Account);
                }
            }

            return null;
        }
    }
}
