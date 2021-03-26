using System.Collections.Generic;
using RedShot.Infrastructure.Uploaders.Clipboard;
using RedShot.Infrastructure.Uploaders.File;
using RedShot.Infrastructure.Uploading.Abstractions;
using RedShot.Infrastructure.Uploading.Common;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp;

namespace RedShot.Infrastructure.Uploading
{
    /// <summary>
    /// This class manages uploading process.
    /// </summary>
    public class UploadingProvider
    {
        /// <summary>
        /// Last file.
        /// </summary>
        public static File LastFile { get; internal set; }

        /// <summary>
        /// Get uploading services.
        /// </summary>
        public static ICollection<IUploadingService> GetUploadingServices()
        {
            return new List<IUploadingService>()
            {
                new FtpUploadingService(),
                new ClipboardUploadingService(),
                new FileUploadingService()
            };
        }
    }
}
