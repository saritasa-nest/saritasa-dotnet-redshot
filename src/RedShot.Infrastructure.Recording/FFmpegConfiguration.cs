using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Abstractions.Configuration;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// FFmpeg configuration.
    /// </summary>
    public class FFmpegConfiguration : IConfigurationOption
    {
        /// <inheritdoc/>
        public string UniqueName => "FFmpegConfiguration";

        /// <summary>
        /// Initialize.
        /// </summary>
        public FFmpegConfiguration()
        {
            FFmpegOptions = new FFmpegOptions();
            AudioOptions = new AudioOptions();
        }

        /// <summary>
        /// Audio options.
        /// </summary>
        public AudioOptions AudioOptions { get; set; }

        /// <summary>
        /// FFmpeg options.
        /// </summary>
        public FFmpegOptions FFmpegOptions { get; set; }
    }
}
