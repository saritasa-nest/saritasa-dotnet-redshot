using Eto.Drawing;
using RedShot.Resources;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Uploading.Common;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploadingService : IUploadingService
    {
        /// <inheritdoc />
        public string Name => "FTP";

        /// <inheritdoc />
        public Bitmap ServiceImage => Icons.Ftp;

        /// <inheritdoc />
        public string About => "Uploads the file to FTP server";

        /// <inheritdoc />
        public IUploader GetUploader()
        {
            var account = FtpAccountProvider.Instance.GetPrimaryFtpAccount();
            return GetFtpUploader(account);
        }

        /// <summary>
        /// Get either FTP or SFTP uploader by specified FTP account.
        /// </summary>
        public BaseFtpUploader GetFtpUploader(FtpAccount account)
        {
            BaseFtpUploader ftpUploader;
            if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
            {
                ftpUploader = new FtpUploader(account);
            }
            else
            {
                ftpUploader = new SftpUploader(account);
            }

            return ftpUploader;
        }

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return fileType switch
            {
                FileType.Image or FileType.Video => true,
                _ => false,
            };
        }
    }
}
