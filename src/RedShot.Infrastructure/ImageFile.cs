using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure
{
    /// <summary>
    /// Image file implementation.
    /// </summary>
    public class ImageFile : IFile
    {
        private readonly Bitmap image;

        /// <summary>
        /// Initializes image file.
        /// </summary>
        public ImageFile(Bitmap image, string filePath, string fileName)
        {
            this.image = image;
            FilePath = filePath;
            FileName = fileName;
        }

        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public FileType FileType => FileType.Image;

        /// <inheritdoc/>
        public string FilePath { get; }

        /// <inheritdoc/>
        public Bitmap GetFilePreview()
        {
            return image;
        }
    }
}
