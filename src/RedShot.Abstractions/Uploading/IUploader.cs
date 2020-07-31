using Eto.Drawing;
using System.IO;

namespace RedShot.Abstractions.Uploading
{
    public interface IUploader
    {
        IUploaderResponse Upload(Stream stream, string filename);

        IUploaderResponse UploadImage(Bitmap bitmap, string filename, ImageFormat format);

        void StopUpload();
    }
}
