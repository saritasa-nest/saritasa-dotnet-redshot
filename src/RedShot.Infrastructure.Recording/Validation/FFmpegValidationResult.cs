using System.Collections.Generic;

namespace RedShot.Infrastructure.Recording.Validation
{
    /// <summary>
    /// FFmpeg validation result.
    /// Uses for checking FFmpeg options on correctness.
    /// </summary>
    public class FFmpegValidationResult
    {
        /// <summary>
        /// List of errors.
        /// </summary>
        public List<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Status of validation.
        /// </summary>
        public bool IsSuccess { get; internal set; }
    }
}
