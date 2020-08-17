using System.Collections.Generic;

namespace RedShot.Helpers.Ffmpeg.Options
{
    public class FFmpegValidationResult
    {
        public List<string> Errors { get; } = new List<string>();

        public bool IsSuccess { get; internal set; }
    }
}
