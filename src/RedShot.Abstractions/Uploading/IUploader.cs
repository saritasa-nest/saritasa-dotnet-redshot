using System.IO;
using Eto.Drawing;

namespace RedShot.Abstractions.Uploading
{
    /// <summary>
    /// Uploader abstraction.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploads stream to some source.
        /// </summary>
        IUploaderResponse Upload(Stream stream, string filename);

        /// <summary>
        /// Uploads image to some source.
        /// </summary>
        IUploaderResponse UploadImage(Bitmap bitmap, string filename, ImageFormat format);

        /// <summary>
        /// Stops uploading.
        /// </summary>
        void StopUpload();
    }
}
