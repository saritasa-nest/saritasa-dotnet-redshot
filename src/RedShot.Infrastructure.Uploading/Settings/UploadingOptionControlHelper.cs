using RedShot.Infrastructure.Abstractions.Uploading;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Infrastructure.Uploading.Settings
{
    internal static class UploadingOptionControlHelper
    {
        public static IEnumerable<UploadingDataGridObject> GetUploadingDataGridObjects(IEnumerable<IUploadingService> uploadingServices, UploadingConfiguration uploadingConfiguration)
        {
            var uploadingDataGridObjects = uploadingServices.Select(u => new UploadingDataGridObject(u.GetType(), u.Name)).ToList();

            foreach (var uploadingObject in uploadingDataGridObjects)
            {
                if (uploadingConfiguration.UploadersTypes.Contains(uploadingObject.UploadingType))
                {
                    uploadingObject.AutoUploadSelected = true;
                }
            }

            return uploadingDataGridObjects;
        }

        public static UploadingConfiguration SetUploadersTypesOnAutoSelect(IEnumerable<UploadingDataGridObject> uploadingDataGridObjects, UploadingConfiguration uploadingConfiguration)
        {
            var onAutoUploadTypes = uploadingDataGridObjects.Where(u => u.AutoUploadSelected).Select(u => u.UploadingType);

            uploadingConfiguration.UploadersTypes.Clear();
            uploadingConfiguration.UploadersTypes.AddRange(onAutoUploadTypes);

            return uploadingConfiguration;
        }
    }
}
