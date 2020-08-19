using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Infrastructure.Uploading.Properties;
using RedShot.Infrastructure.Uploaders.Ftp.Forms;

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

        /// <summary>
        /// Creates either FTP or STPS uploader.
        /// </summary>
        public IUploader GetUploader()
        {
            using (var form = new FtpUploaderForm())
            {
                if (form.ShowModal() == Eto.Forms.DialogResult.Ok)
                {
                    var account = form.SelectedAccount;

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
                }
            }

            return null;
        }

        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }
    }
}
