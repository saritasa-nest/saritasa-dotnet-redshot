using System.IO;
using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;

namespace RedShot.Infrastructure
{
    public class ImageFile : IFile
    {
        private readonly Bitmap image;

        public ImageFile(Bitmap image, string filePath, string fileName)
        {
            this.image = image;
            FilePath = filePath;
            FileName = fileName;
        }

        public string FileName { get; }

        public FileType FileType => FileType.Image;

        public string FilePath { get; }

        public Bitmap GetFilePreview()
        {
            return image;
        }

        public Stream GetStream()
        {
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);

            return stream;
        }
    }
}
