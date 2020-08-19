using Eto.Drawing;
using RedShot.Abstractions;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Properties;

namespace RedShot.Infrastructure.Uploaders.File
{
    public class FileUploadingService : IUploadingService
    {
        public string ServiceName => "File";

        public Image ServiceImage
        {
            get
            {
                return new Bitmap(Resources.download);
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
