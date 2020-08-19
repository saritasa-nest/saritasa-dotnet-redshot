using Eto.Drawing;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using System.IO;

namespace RedShot.Infrastructure.Recording
{
    public class VideoFile : IFile
    {
        private readonly string videoPath;

        public VideoFile(string name, string videoPath)
        {
            FileName = name;
            this.videoPath = videoPath;
        }

        public string FileName { get; }

        public FileType FileType => FileType.Video;

        public string GetFilePath()
        {
            return videoPath;
        }

        public Bitmap GetFilePreview()
        {
            using (var stream = new MemoryStream())
            {
                Properties.Resources.multimedia.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return new Bitmap(stream);
            }
        }

        public Stream GetStream()
        {
            return File.Open(GetFilePath(), FileMode.Open);
        }
    }
}
