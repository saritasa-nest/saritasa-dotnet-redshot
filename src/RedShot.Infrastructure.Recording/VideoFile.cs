using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Resources;

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
        public string FileName { get; set; }

        /// <inheritdoc/>
        public FileType FileType => FileType.Video;

        /// <inheritdoc/>
        public string FilePath { get; }

        /// <inheritdoc/>
        public Bitmap GetFilePreview()
        {
            return Icons.Video;
        }
    }
}
