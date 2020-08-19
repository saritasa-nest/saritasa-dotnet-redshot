using Eto.Drawing;
using RedShot.Abstractions;
using RedShot.Abstractions.Uploading;
using RedShot.Upload.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
