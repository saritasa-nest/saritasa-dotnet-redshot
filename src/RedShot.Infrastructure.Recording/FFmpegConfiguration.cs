using System;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;

namespace RedShot.Infrastructure.Recording
{
    public class FFmpegConfiguration : IConfigurationOption
    {
        public FFmpegConfiguration()
        {
            UniqueName = "FFmpeg configuration";
            Options = new FFmpegOptions();
        }

        public string UniqueName { get; }

        public FFmpegOptions Options { get; set; }

        public FFmpegConfiguration Clone()
        {
            var clone = (FFmpegConfiguration)MemberwiseClone();
            clone.Options = Options.Clone();

            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
