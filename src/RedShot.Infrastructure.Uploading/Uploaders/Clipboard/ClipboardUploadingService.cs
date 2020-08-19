using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Uploading.Properties;
using System;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    class FileUploadingService : IUploadingService
    {
        public string ServiceName => "Clipboard";

        public Image ServiceImage
        {
            get
            {
                return new Bitmap(Resources.form);
            }
        }

        public bool CheckOnSupporting(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Image:
                    return true;
            }

            return false;
        }

        public IUploader GetUploader()
        {
            throw new NotImplementedException();
        }
    }
}
