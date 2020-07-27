using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Upload.Abstractions
{
    public interface IUploaderResponse
    {
        IEnumerable<string> Errors { get; }

        bool IsSuccess { get; }

        string FileUrl { get; }

        int StatusCode { get; }
    }
}
