using RedShot.Upload.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedShot.Upload.Basics
{
    public class BaseUploaderResponse : IUploaderResponse
    {
        public BaseUploaderResponse(bool isSuccess, string fileUrl = default, int statusCode = default, IEnumerable<string> errors = default)
        {
            IsSuccess = isSuccess;
            FileUrl = fileUrl;
            StatusCode = statusCode;
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }

        public bool IsSuccess { get; }

        public string FileUrl { get; }

        public int StatusCode { get; }
    }
}
