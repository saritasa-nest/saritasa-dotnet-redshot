using System;

namespace RedShot.Infrastructure.Uploading.Common
{
    /// <summary>
    /// Uploading finished event arguments.
    /// </summary>
    public class UploadingFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UploadingFinishedEventArgs(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Uploading file.
        /// </summary>
        public string FileName { get; }
    }
}
