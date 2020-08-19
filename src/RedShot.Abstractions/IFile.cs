using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Abstractions
{
    public interface IFile
    {
        Stream GetStream();

        string GetFilePath();

        string FileName { get; }

        Bitmap GetFilePreview();

        FileType FileType { get; }
    }
}
