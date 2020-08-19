using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure.Uploaders.File
{
    public class FileUploadingService : IUploadingService
    {
        public string ServiceName => "File";

        public Image ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Download);
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
