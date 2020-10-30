using System.Collections.Generic;
using System.IO;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Uploading.Uploaders.Ftp.Models;

namespace RedShot.Infrastructure.Uploading.Uploaders.Ftp
{
    /// <summary>
    /// FTP helper.
    /// </summary>
    internal static class FtpHelper
    {
        /// <summary>
        /// Get full file name.
        /// </summary>
        public static string GetFullFileName(IFile file)
        {
            return $"{file.FileName}{Path.GetExtension(file.FilePath)}";
        }
    }
}
