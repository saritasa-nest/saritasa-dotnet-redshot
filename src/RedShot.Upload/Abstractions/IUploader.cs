using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Upload.Abstractions
{
    public interface IUploader
    {
        IUploaderResponse Upload(Stream stream, string filename);
    }
}
