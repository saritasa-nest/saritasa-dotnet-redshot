using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Infrastructure.Domain.Files
{
    /// <summary>
    /// File types.
    /// </summary>
    public enum FileType
    {
        Image = 1,
        Video = 2
    }

    /// <summary>
    /// File.
    /// </summary>
    public class File
    {
        private readonly byte[] fileBytes;

        public File(byte[] fileBytes, FileType fileType)
        {
            this.fileBytes = fileBytes;
            FileType = fileType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="fileType">File type.</param>
        public File(string filePath, FileType fileType)
        {
            FilePath = filePath;
            FileType = fileType;
        }

        /// <summary>
        /// Get file stream.
        /// </summary>
        public Stream GetStream()
        {
            if (fileBytes is not null)
            {
                return new MemoryStream(fileBytes);
            }

            return System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// File path.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// File type.
        /// </summary>
        public FileType FileType { get; }
    }
}
