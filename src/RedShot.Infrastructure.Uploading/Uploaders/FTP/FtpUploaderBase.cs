using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Infrastructure.Formatting;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP uploader.
    /// </summary>
    public abstract class FtpUploaderBase : IUploader
    {
        /// <summary>
        /// FTP account.
        /// </summary>
        protected readonly FtpAccount ftpAccount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ftpAccount">FTP account.</param>
        public FtpUploaderBase(FtpAccount ftpAccount)
        {
            this.ftpAccount = ftpAccount;
        }

        /// <summary>
        /// Upload file to destination resource.
        /// </summary>
        public async Task<UploadResult> UploadAsync(Common.File file, CancellationToken cancellationToken)
        {
            var subFolderPath = ftpAccount.Directory;

            var fileName = FormatManager.GetFormattedName();
            var fullFileName = GetFullFileName(fileName, file);

            var path = UrlHelper.CombineUrl(subFolderPath, fullFileName);

            try
            {
                await using var fileStream = file.GetStream();
                await UploadStreamAsync(fileStream, path, cancellationToken);
            }
            catch (Exception e)
            {
                return UploadResult.Error(e.Message);
            }

            SaveFileUrlToClipboard(fullFileName);
            return UploadResult.Successful;
        }

        /// <summary>
        /// Upload file stream.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="remotePath">Remote path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        protected abstract Task UploadStreamAsync(Stream stream, string remotePath, CancellationToken cancellationToken = default);

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

        /// <summary>
        /// Save file URL to clipboard.
        /// </summary>
        /// <param name="fullFileName">Full file name.</param>
        protected void SaveFileUrlToClipboard(string fullFileName)
        {
            var link = ftpAccount.GetFormatLink(fullFileName);

            Clipboard.Instance.Clear();
            Clipboard.Instance.Text = link;

            NotifyHelper.Notify("File uploaded to FTP server.\n\nFile link saved to clipboard.", "RedShot", NotifyStatus.Success);
        }

        /// <summary>
        /// Get full file name.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="file">File.</param>
        protected string GetFullFileName(string fileName, Common.File file)
        {
            return $"{fileName}{Path.GetExtension(file.FilePath)}";
        }
    }
}
