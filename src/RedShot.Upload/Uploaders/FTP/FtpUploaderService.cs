using Eto.Drawing;
using RedShot.Helpers.FtpModels;
using RedShot.Upload.Abstractions;
using RedShot.Upload.Basics;
using System;

namespace RedShot.Upload.Uploaders.FTP
{
    public class FtpUploaderService : IUploaderService
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
            var account = GetFtpAccount();

            if (account != null)
            {
                if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
                {
                    return new Ftp(account);
                }
                else if (account.Protocol == FtpProtocol.SFTP)
                {
                    return new Sftp(account);
                }
            }

            return null;
        }

        private FtpAccount GetFtpAccount()
        {
            return null;
        }
    }
}
