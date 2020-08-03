using RedShot.Abstractions.Uploading;
using System.Collections.Generic;

namespace RedShot.Upload.Basics
{
    /// <summary>
    /// Simple response object.
    /// </summary>
    public class BaseUploaderResponse : IUploaderResponse
    {
        /// <summary>
        /// Initialize properties.
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="fileUrl"></param>
        /// <param name="statusCode"></param>
        /// <param name="errors"></param>
        public BaseUploaderResponse(bool isSuccess, string fileUrl = default, int statusCode = default, IEnumerable<string> errors = default)
        {
            IsSuccess = isSuccess;
            FileUrl = fileUrl;
            StatusCode = statusCode;
            Errors = errors;
        }

        /// <summary>
        /// Errors collection.
        /// </summary>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Result.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// File url if exists.
        /// </summary>
        public string FileUrl { get; }

        /// <summary>
        /// Status code of the result.
        /// </summary>
        public int StatusCode { get; }
    }
}
