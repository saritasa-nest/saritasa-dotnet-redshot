using RedShot.Infrastructure.Abstractions.Interfaces;
using RedShot.Infrastructure.Abstractions.Interfaces.Ftp;
using RedShot.Infrastructure.Domain.Files;
using RedShot.Infrastructure.Domain.Ftp;
using RedShot.Infrastructure.DomainServices.Common.Helpers;
using Saritasa.Tools.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using File = RedShot.Infrastructure.Domain.Files.File;

namespace RedShot.Infrastructure.DomainServices.Services.Ftp
{
    internal abstract class BaseFtpClient : IFtpClient
    {
        private readonly IFormatNameService formatService;
        private readonly FtpAccount ftpAccount;
        private readonly SemaphoreSlim clientSemaphore;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ftpAccount">FTP account.</param>
        public BaseFtpClient(IFormatNameService formatService, FtpAccount ftpAccount)
        {
            this.formatService = formatService;
            this.ftpAccount = ftpAccount;

            clientSemaphore = new SemaphoreSlim(0, 1);
        }

        /// <summary>
        /// Upload file to destination resource.
        /// </summary>
        public async Task UploadAsync(File file, CancellationToken cancellationToken)
        {
            await clientSemaphore.WaitAsync();

            try
            {
                var subFolderPath = ftpAccount.Directory;

                var fileName = formatService.GetFormattedName();
                var fullFileName = GetFullFileName(fileName, file);

                var path = UrlHelper.CombineUrl(subFolderPath, fullFileName);

                await using var fileStream = file.GetStream();
                await UploadStreamAsync(fileStream, path, cancellationToken);
            }
            catch (Exception e)
            {
                throw new DomainException("An error occurred during uploading file.", e);
            }
            finally
            {
                clientSemaphore.Release();
            }
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
            await clientSemaphore.WaitAsync();

            try
            {
                return await ConnectAsync(cancellationToken);
            }
            catch
            {
                return false;
            }
            finally
            {
                clientSemaphore.Release();
            }
        }


        /// <summary>
        /// Get full file name.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="file">File.</param>
        protected string GetFullFileName(string fileName, File file)
        {
            return $"{fileName}{Path.GetExtension(file.FilePath)}";
        }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
