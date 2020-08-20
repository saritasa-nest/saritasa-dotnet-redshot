using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure.Uploaders.File
{
    public class FileUploadingService : IUploadingService
    {
        public string ServiceName => "File";

        public Bitmap ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Properties.Resources.Folder);
            }
        }

        public bool CheckOnSupporting(FileType fileType)
        {
            return true;
        }

        public IUploader GetUploader()
        {
            return new FileUploader();
        }
    }
}
