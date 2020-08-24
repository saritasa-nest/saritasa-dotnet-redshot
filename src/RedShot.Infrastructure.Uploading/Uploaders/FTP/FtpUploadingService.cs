using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.DataTransfer.Ftp;
using RedShot.Infrastructure.Uploaders.Ftp.Forms;

namespace RedShot.Infrastructure.Uploaders.Ftp
{
    /// <summary>
    /// Manages FTP uploading.
    /// </summary>
    public class FtpUploadingService : IUploadingService
    {
        /// <inheritdoc cref="IUploadingService"/>
        public string ServiceName => "FTP / SFTP / FTPS";

        /// <summary>
        /// Selected FTP account.
        /// </summary>
        public FtpAccount Account { get; }

        /// <inheritdoc cref="IUploadingService"/>
        public Bitmap ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Properties.Resources.Ftp);
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

        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }
    }
}
