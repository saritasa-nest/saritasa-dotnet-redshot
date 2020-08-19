using Eto.Drawing;
using RedShot.Infrastructure.Abstractions.Uploading;
using System.IO;


namespace RedShot.Infrastructure.Abstractions
{
    public interface IFile
    {
        Stream GetStream();

        string FilePath { get; }

        string FileName { get; }

        Bitmap GetFilePreview();

        FileType FileType { get; }
    }
}
