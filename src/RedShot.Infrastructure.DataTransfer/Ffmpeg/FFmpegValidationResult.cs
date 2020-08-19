using System.Collections.Generic;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg
{
    public class FFmpegValidationResult
    {
        public List<string> Errors { get; } = new List<string>();

        public bool IsSuccess { get; internal set; }
    }
}
