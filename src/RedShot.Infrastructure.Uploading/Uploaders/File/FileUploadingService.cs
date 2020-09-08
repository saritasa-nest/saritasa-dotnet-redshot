using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Notifying;

namespace RedShot.Infrastructure.Uploaders.File
{
    /// <summary>
    /// File uploading service.
    /// </summary>
    internal class FileUploadingService : IUploadingService
    {
        /// <inheritdoc />
        public string Name => "File";

        /// <inheritdoc />
        public Bitmap ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Properties.Resources.Folder);
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
            return new FileUploader();
        }

        /// <inheritdoc />
        public void OnUploaded(IFile file)
        {
            NotifyHelper.Notify("The file has been saved.", "RedShot", NotifyStatus.Success);
        }
    }
}
