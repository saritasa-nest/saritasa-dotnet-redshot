using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;
using RedShot.Resources;

namespace RedShot.Infrastructure.Uploaders.File
{
    /// <summary>
    /// File uploading service.
    /// </summary>
    public class FileUploadingService : IUploadingService
    {
        /// <inheritdoc />
        public string Name => "File";

        /// <inheritdoc />
        public Bitmap ServiceImage
        {
            get
            {
                return Icons.Folder;
            }
        }

        /// <inheritdoc />
        public string About => "Uploads the file to specified folder";

        /// <inheritdoc />
        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }

        /// <inheritdoc />
        public IUploader GetUploader()
        {
            var uploader = new FileUploader();
            uploader.UploadingFinished += FileUploaderUploadingFinished;

            return uploader;
        }

        private void FileUploaderUploadingFinished(object sender, UploadingFinishedEventArgs e)
        {
            NotifyHelper.Notify($"The {e.UploadingFile.FileName} file has been saved.", "RedShot", NotifyStatus.Success);
        }
    }
}
