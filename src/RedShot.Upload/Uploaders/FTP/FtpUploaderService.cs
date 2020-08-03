using System;
using Eto.Drawing;
using RedShot.Helpers.FtpModels;
using RedShot.Abstractions.Uploading;

namespace RedShot.Upload.Uploaders.FTP
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploaderService : IUploaderService
    {
        public FtpUploaderService(FtpAccount account)
        {
            Account = account;
        }

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount Account { get; }

        public string ServiceIdentifier => "FTP/SFTP/FTPS";

        public string ServiceName => "FTP/SFTP/FTPS";

        public Icon ServiceIcon { get; }

        public Image ServiceImage { get; }

        public bool CheckConfig(IUploaderConfig config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates either FTP or STPS uploader.
        /// </summary>
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
