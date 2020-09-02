using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Infrastructure.Uploaders.Ftp.Forms;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    internal class FtpUploadingService : IUploadingService
    {
        /// <inheritdoc />
        public string Name => "FTP/SFTP/FTPS uploading service";

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount Account { get; }

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
                    var account = form.SelectedAccount;
                    var name = form.FileName;

                    if (account != null)
                    {
                        if (account.Protocol == FtpProtocol.FTP || account.Protocol == FtpProtocol.FTPS)
                        {
                            return new Ftp(account, name);
                        }
                        else if (account.Protocol == FtpProtocol.SFTP)
                        {
                            return new Sftp(account, name);
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
    }
}
