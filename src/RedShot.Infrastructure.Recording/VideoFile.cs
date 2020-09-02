﻿using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Video file.
    /// </summary>
    public class VideoFile : IFile
    {
        /// <summary>
        /// Initializes video file.
        /// </summary>
        public VideoFile(string name, string path)
        {
            FileName = name;
            FilePath = path;
        }

        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public FileType FileType => FileType.Video;

        /// <inheritdoc/>
        public string FilePath { get; }

        /// <inheritdoc/>
        public Bitmap GetFilePreview()
        {
            return new Bitmap(Resources.Properties.Resources.Video);
        }

        /// <inheritdoc/>
        public Stream GetStream()
        {
            return File.Open(FilePath, FileMode.Open);
        }
    }
}
