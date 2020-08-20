﻿using Eto.Drawing;

namespace RedShot.Infrastructure.Abstractions.Uploading
{
    /// <summary>
    /// Upload service abstraction.
    /// Will be changed.
    /// </summary>
    public interface IUploadingService
    {
        /// <summary>
        /// Service name.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Service Eto Image.
        /// </summary>
        Bitmap ServiceImage { get; }

        /// <summary>
        /// Creates IUploader instance.
        /// </summary>
        IUploader GetUploader();

        bool CheckOnSupporting(FileType fileType);
    }
}
