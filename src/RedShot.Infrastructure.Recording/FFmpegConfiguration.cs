using System;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// FFmpeg configuration.
    /// </summary>
    public class FFmpegConfiguration : IConfigurationOption
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public FFmpegConfiguration()
        {
            UniqueName = "FFmpeg configuration";
            Options = new FFmpegOptions();
        }

        /// <inheritdoc/>
        public string UniqueName { get; }

        /// <summary>
        /// FFmpeg options.
        /// </summary>
        public FFmpegOptions Options { get; set; }

        /// <summary>
        /// Clone.
        /// </summary>
        public FFmpegConfiguration Clone()
        {
            var clone = (FFmpegConfiguration)MemberwiseClone();
            clone.Options = Options.Clone();

            return clone;
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
