using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Forms;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    public abstract class BaseFtpUploader : IUploader
    {
        protected readonly FtpAccount ftpAccount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ftpAccount">FTP account.</param>
        public BaseFtpUploader(FtpAccount ftpAccount)
        {
            this.ftpAccount = ftpAccount;
        }

        /// <summary>
        /// Uploading flag.
        /// </summary>
        public bool IsUploading { get; protected set; }

        /// <summary>
        /// Buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// Upload file to destination resource.
        /// </summary>
        public abstract Task UploadAsync(Common.File file, CancellationToken cancellationToken);

        /// <summary>
        /// Connect to FTP server.
        /// </summary>
        protected abstract Task<bool> ConnectAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Test connection.
        /// </summary>
        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await ConnectAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
        }

        protected void SaveFileUrlToClipboard(string fullFileName)
        {
            var link = ftpAccount.GetFormatLink(fullFileName);

            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = link;

            NotifyHelper.Notify("File uploaded to FTP server.\n\nFile link saved to clipboard.", "RedShot", NotifyStatus.Success);
        }

        protected string GetFullFileName(string fileName, Common.File file)
        {
            return $"{fileName}{Path.GetExtension(file.FilePath)}";
        }
    }
}
