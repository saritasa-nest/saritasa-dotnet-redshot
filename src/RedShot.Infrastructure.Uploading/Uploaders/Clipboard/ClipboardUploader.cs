using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Basics;

namespace RedShot.Infrastructure.Uploaders.Clipboard
{
    public class ClipboardUploader : IUploader
    {
        public IUploadingResponse Upload(IFile file)
        {
            Eto.Forms.Clipboard.Instance.Clear();
            Eto.Forms.Clipboard.Instance.Image = file.GetFilePreview();

            return new BaseUploadingResponse(true);
        }
    }
}
