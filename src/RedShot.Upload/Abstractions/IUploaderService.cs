using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Upload.Abstractions
{
    public interface IUploaderService
    {
        string ServiceIdentifier { get; }

        string ServiceName { get; }

        Icon ServiceIcon { get; }

        Image ServiceImage { get; }

        bool CheckConfig(IUploaderConfig config);

        IUploader CreateUploader();
    }
}
