﻿using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using System.IO;

namespace RedShot.Infrastructure.Abstractions
{
    /// <summary>
    /// Abstraction for files.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gives stream of the file.
        /// </summary>
        public Stream GetStream()
        {
            return File.Open(FilePath, FileMode.Open);
        }

        /// <summary>
        /// File path of the file.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// File name of the file.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gives file preview in Bitmap.
        /// </summary>
        Bitmap GetFilePreview();

        /// <summary>
        /// File type of the file.
        /// </summary>
        FileType FileType { get; }
    }
}
