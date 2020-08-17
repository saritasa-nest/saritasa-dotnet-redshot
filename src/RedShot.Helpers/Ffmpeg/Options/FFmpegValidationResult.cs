using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Helpers.Ffmpeg
{
    public class FFmpegValidationResult
    {
        public List<string> Errors { get; } = new List<string>();

        public bool IsSuccess { get; internal set; }
    }
}
