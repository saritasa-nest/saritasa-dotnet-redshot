using System;

namespace RedShot.Infrastructure.Abstractions.Uploading
{
    /// <summary>
    /// Uploading finished event args.
    /// </summary>
    public class UploadingFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Uploading file.
        /// </summary>
        public IFile UploadingFile { get; set; }

        /// <summary>
        /// Create new uploading finished event args.
        /// </summary>
        public static UploadingFinishedEventArgs CreateNew(IFile file)
        {
            return new UploadingFinishedEventArgs()
            {
                UploadingFile = file
            };
        }
    }
}
