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
        /// <summary>
        /// Uploading flag.
        /// </summary>
        public bool IsUploading { get; protected set; }

        /// <summary>
        /// Buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// Stop uploading requested flag.
        /// </summary>
        public bool StopUploadRequested { get; protected set; }

        /// <summary>
        /// Occures when uploading has finished.
        /// </summary>
        public virtual event EventHandler Uploaded;

        /// <summary>
        /// Occures when uploading has stoped.
        /// </summary>
        public virtual event EventHandler UploadStoped;

        /// <summary>
        /// Occures when uploading has started.
        /// </summary>
        public virtual event EventHandler UploadStarted;

        /// <summary>
        /// Occures by uplod stopping.
        /// </summary>
        public virtual event EventHandler UploadError;

        /// <summary>
        /// Converts image to memory stream and then uploads it.
        /// </summary>
        public virtual IUploaderResponse UploadImage(Bitmap bitmap, string filename, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return Upload(stream, filename);
            }
        }

        /// <summary>
        /// Uploads byte array data.
        /// </summary>
        public virtual IUploaderResponse UploadData(byte[] data, string filename)
        {
            using (var stream = new MemoryStream(data, false))
            {
                return Upload(stream, filename);
            }
        }

        /// <summary>
        /// Uploads file.
        /// </summary>
        public virtual IUploaderResponse UploadFile(string localPath, string filename)
        {
            using (var stream = new FileStream(localPath, FileMode.Open))
            {
                return Upload(stream, filename);
            }
        }

        /// <summary>
        /// Uploads text.
        /// </summary>
        public virtual IUploaderResponse UploadText(string text, string filename)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text), false))
            {
                return Upload(stream, filename);
            }
        }

        /// <summary>
        /// Stops upload.
        /// </summary>
        public abstract void StopUpload();

        /// <summary>
        /// Upload stream to destination resource.
        /// </summary>
        public abstract IUploaderResponse Upload(Stream stream, string filename);
    }
}
