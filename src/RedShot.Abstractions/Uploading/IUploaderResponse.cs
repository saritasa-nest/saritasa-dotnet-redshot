using System.Collections.Generic;

namespace RedShot.Abstractions.Uploading
{
    public interface IUploaderResponse
    {
        IEnumerable<string> Errors { get; }

        bool IsSuccess { get; }

        string FileUrl { get; }

        int StatusCode { get; }
    }
}
