namespace RedShot.Infrastructure.Abstractions.Uploading
{
    /// <summary>
    /// Upload abstraction.
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Uploads stream to some source.
        /// </summary>
        IUploadingResponse Upload(IFile file);
    }
}
