using System;
using System.Collections.Generic;
using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Recording.Ffmpeg.Devices;

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
            FFmpegOptions = new FFmpegOptions();
            AudioOptions = new AudioOptions();
        }

        /// <inheritdoc/>
        public string UniqueName { get; }

        public AudioOptions AudioOptions { get; set; }

        /// <summary>
        /// FFmpeg options.
        /// </summary>
        public FFmpegOptions FFmpegOptions { get; set; }

        /// <summary>
        /// Clone.
        /// </summary>
        public FFmpegConfiguration Clone()
        {
            var clone = (FFmpegConfiguration)MemberwiseClone();
            clone.FFmpegOptions = FFmpegOptions.Clone();

            return clone;
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
