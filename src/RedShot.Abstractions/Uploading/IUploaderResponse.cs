using System.Collections.Generic;

namespace RedShot.Abstractions.Uploading
{
    /// <summary>
    /// Response abstraction.
    /// </summary>
    public interface IUploaderResponse
    {
        /// <summary>
        /// Errors collection.
        /// </summary>
        IEnumerable<string> Errors { get; }

        /// <summary>
        /// Result.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// File url if exists.
        /// </summary>
        string FileUrl { get; }

        /// <summary>
        /// Status code of the result.
        /// </summary>
        int StatusCode { get; }
    }
}
