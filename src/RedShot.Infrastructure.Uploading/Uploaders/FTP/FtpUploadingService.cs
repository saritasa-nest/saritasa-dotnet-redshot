using Eto.Drawing;
using System;
using RedShot.Uploaders.FTP.Forms;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Infrastructure.Uploading.Properties;
using RedShot.Infrastructure.Configuration;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploadingService : IUploadingService
    {
        /// <inheritdoc cref="IUploadingService"/>.
        public string ServiceName => "FTP / SFTP / FTPS";

        /// <summary>
        /// Inits FTP uploader service.
        /// </summary>
        public FtpUploadingService(FtpAccount account)
        {
            Account = account;
        }

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount Account { get; }

        /// <inheritdoc cref="IUploadingService"/>
        public Image ServiceImage
        {
            get
            {
                return new Bitmap(Resources.ftp);
            }
        }

        private FtpConfiguration GetFtpConfiguration()
        {
            var config = ConfigurationManager.GetSection<FtpConfiguration>();

            if (config != null)
            {
                return (FtpConfiguration)config;
            }
            else
            {

            }
        }

        /// <summary>
        /// Runs view for FTP uploading.
        /// </summary>
        private FtpAccount RunAccountSelectionView()
        {
            var form = new FtpUploaderForm();

            form.ShowModal();

            form.Se
        }

        /// <summary>
        /// Creates either FTP or STPS uploader.
        /// </summary>
        public IUploader GetUploader()
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

        public bool CheckOnSupporting(FileType fileType)
        {
            throw new NotImplementedException();
        }
    }
}
