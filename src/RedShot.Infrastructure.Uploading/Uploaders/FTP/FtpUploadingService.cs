using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Resources;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploadingService : IUploadingService
    {
        private FtpAccount account;

        /// <inheritdoc />
        public string Name => "FTP";

        /// <inheritdoc />
        public Bitmap ServiceImage => Icons.Ftp;

        /// <inheritdoc />
        public string About => "Uploads the file to FTP server";

        /// <summary>
        /// Creates either FTP or SFTP uploader.
        /// </summary>
        public IUploader GetUploader()
        {
            var account = FtpAccountManager.GetPrimaryFtpAccount();

            if (account == null)
            {
                account = FtpAccountManager.GetFtpAccountManually();
            }

            return GetFtpUploader(account);
        }

        /// <summary>
        /// Get either FTP or SFTP uploader by specified FTP account.
        /// </summary>
        public BaseFtpUploader GetFtpUploader(FtpAccount account)
        {
            if (account == null)
            {
                return null;
            }
            else
            {
                this.account = account;
            }

            BaseFtpUploader ftpUploader;
            if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
            {
                ftpUploader = new FtpUploader(account);
            }
            else
            {
                ftpUploader = new SftpUploader(account);
            }

            ftpUploader.UploadingFinished += FtpUploaderUploadingFinished;
            return ftpUploader;
        }

        private void FtpUploaderUploadingFinished(object sender, UploadingFinishedEventArgs e)
        {
            var link = account.GetFormatLink(FtpHelper.GetFullFileName(e.UploadingFile));

            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = link;

            NotifyHelper.Notify("File uploaded to FTP server.\n\nFile link saved to clipboard.", "RedShot", NotifyStatus.Success);
        }

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }
    }
}
