using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using System.IO;

namespace RedShot.Infrastructure.Recording
{
    public class VideoFile : IFile
    {
        public VideoFile(string name, string videoPath)
        {
            FileName = name;
            FilePath = videoPath;
        }

        public string FileName { get; }

        public FileType FileType => FileType.Video;

        public string FilePath { get; }

        public Bitmap GetFilePreview()
        {
            using (var stream = new MemoryStream())
            {
                return new Bitmap(Resources.Properties.Resources.Video);
            }
        }

        public Stream GetStream()
        {
            return File.Open(FilePath, FileMode.Open);
        }
    }
}
