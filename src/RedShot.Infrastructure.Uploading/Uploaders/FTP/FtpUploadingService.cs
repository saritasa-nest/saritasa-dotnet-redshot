using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Uploaders.Ftp.Models;
using RedShot.Infrastructure.Uploaders.Ftp.Forms;
using RedShot.Infrastructure.Abstractions;
using System.IO;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    internal class FtpUploadingService : IUploadingService
    {
        private FtpAccount account;

        /// <inheritdoc />
        public string Name => "FTP/SFTP/FTPS uploading service";

        /// <inheritdoc />
        public Bitmap ServiceImage => new Bitmap(Resources.Properties.Resources.Ftp);

        /// <inheritdoc />
        public string About => "Uploads the file to FTP server";

        /// <summary>
        /// Creates either FTP or SFTP uploader.
        /// </summary>
        public IUploader GetUploader()
        {
            using (var form = new FtpUploaderForm())
            {
                if (form.ShowModal() == Eto.Forms.DialogResult.Ok)
                {
                    account = form.SelectedAccount;
                    var name = form.FileName;

                    if (account != null)
                    {
                        if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
                        {
                            return new FtpUploader(account, name);
                        }
                        else if (account.Protocol == FtpProtocol.SFTP)
                        {
                            return new SftpUploader(account, name);
                        }
                    }
                }
            }

            return null;
        }

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }

        /// <inheritdoc />
        public void OnUploaded(IFile file)
        {
            var link = account.GetFormatLink(Path.GetFileName(file.FilePath));

            Eto.Forms.Clipboard.Instance.Clear();
            Eto.Forms.Clipboard.Instance.Text = link;

            NotifyHelper.Notify("The file has been uploaded to your FTP server.\nRedShot has saved format link to clipboard.", "RedShot", NotifyStatus.Success);
        }
    }
}
