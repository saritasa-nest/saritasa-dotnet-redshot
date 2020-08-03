using Eto.Drawing;
using RedShot.Abstractions.Uploading;
using System;
using System.IO;
using System.Text;

namespace RedShot.Upload.Basics
{
    /// <summary>
    /// Base upload functions.
    /// </summary>
    public abstract class BaseUploader : IUploader
    {
        public bool IsUploading { get; protected set; }

        public int BufferSize { get; set; } = 8192;

        public bool StopUploadRequested { get; protected set; }

        public virtual event EventHandler Uploaded;

        public virtual event EventHandler UploadStoped;

        public virtual event EventHandler UploadStarted;

        public virtual event EventHandler UploadError;

        public virtual IUploaderResponse UploadImage(Bitmap bitmap, string filename, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return Upload(stream, filename);
            }
        }

        public virtual IUploaderResponse UploadData(byte[] data, string filename)
        {
            using (var stream = new MemoryStream(data, false))
            {
                return Upload(stream, filename);
            }
        }

        public virtual IUploaderResponse UploadFile(string localPath, string filename)
        {
            using (var stream = new FileStream(localPath, FileMode.Open))
            {
                return Upload(stream, filename);
            }
        }

        public virtual IUploaderResponse UploadText(string text, string filename)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text), false))
            {
                return Upload(stream, filename);
            }
        }

        public abstract void StopUpload();

        public abstract IUploaderResponse Upload(Stream stream, string filename);
    }
}
