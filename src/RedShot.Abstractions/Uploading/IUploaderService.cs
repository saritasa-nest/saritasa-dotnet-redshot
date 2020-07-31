using Eto.Drawing;

namespace RedShot.Abstractions.Uploading
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
