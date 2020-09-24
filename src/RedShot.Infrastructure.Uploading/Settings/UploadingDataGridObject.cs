using System;

namespace RedShot.Infrastructure.Uploading.Settings
{
    internal class UploadingDataGridObject
    {
        public UploadingDataGridObject(Type uploadingType, string name, bool autoUploadSelected = false)
        {
            UploadingType = uploadingType;
            AutoUploadSelected = autoUploadSelected;
            UploaderName = name;
        }

        public Type UploadingType { get; }

        public bool AutoUploadSelected { get; set; }

        public string UploaderName { get; }
    }
}
