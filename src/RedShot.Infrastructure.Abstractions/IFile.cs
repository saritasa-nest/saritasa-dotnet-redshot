using Eto.Drawing;
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
        Stream GetStream();

        /// <summary>
        /// File path of the file.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// File name of the file.
        /// </summary>
        string FileName { get; }

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
