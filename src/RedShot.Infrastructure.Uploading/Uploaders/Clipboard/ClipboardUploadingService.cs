using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Uploaders.File;
using System;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    class FileUploadingService : IUploadingService
    {
        public string ServiceName => "Clipboard";

        public Bitmap ServiceImage
        {
            get
            {
                return new Bitmap(Resources.Properties.Resources.Form);
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
            return new ClipboardUploader();
        }
    }
}
