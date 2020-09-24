using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Forms;
using RedShot.Resources;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    internal class FtpUploadingService : IUploadingService
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
            account = GetFtpAccount();

            if (account != null)
            {
                return GetFtpUploader(account);
            }

            return null;
        }

        private FtpAccount GetFtpAccount()
        {
            using (var form = new FtpUploaderForm())
            {
                if (form.ShowModal() == DialogResult.Ok)
                {
                    return form.SelectedAccount;
                }
            }

            return null;
        }

        /// <summary>
        /// Get FTP uploader.
        /// </summary>
        internal BaseFtpUploader GetFtpUploader(FtpAccount account)
        {
            if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
            {
                return new FtpUploader(account);
            }
            else
            {
                return new SftpUploader(account);
            }
        }

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }

        /// <inheritdoc />
        public void OnUploaded(IFile file)
        {
            var link = account.GetFormatLink(FtpHelper.GetFullFileName(file));

            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = link;

            NotifyHelper.Notify("The file has been uploaded to your FTP server.\n\nRedShot has saved the file link in clipboard.", "RedShot", NotifyStatus.Success);
        }
    }
}
