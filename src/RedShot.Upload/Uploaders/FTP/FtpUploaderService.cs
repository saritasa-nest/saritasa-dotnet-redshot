using Eto.Drawing;
using RedShot.Helpers.FtpModels;
using RedShot.Abstractions.Uploading;
using System;

namespace RedShot.Upload.Uploaders.FTP
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploaderService : IUploaderService
    {
        /// <summary>
        /// Inits FTP uploader service.
        /// </summary>
        public FtpUploaderService(FtpAccount account)
        {
            Account = account;
        }

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount Account { get; }

        /// <inheritdoc cref="IUploaderService"/>.
        public string ServiceName => "FTP/SFTP/FTPS";

        /// <inheritdoc cref="IUploaderService"/>.
        public Icon ServiceIcon { get; }

        /// <inheritdoc cref="IUploaderService"/>.
        public Image ServiceImage { get; }

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

            throw new InvalidOperationException("FTP account is null");
        }
    }
}
