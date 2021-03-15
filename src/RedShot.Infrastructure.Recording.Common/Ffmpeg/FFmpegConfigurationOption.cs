using RedShot.Infrastructure.Abstractions.Configuration;

namespace RedShot.Infrastructure.Recording.Common.Ffmpeg
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
            UniqueName = "FFmpegConfiguration";
            FFmpegOptions = new FFmpegOptions();
            AudioOptions = new AudioOptions();
        }

        /// <inheritdoc/>
        public string UniqueName { get; }        
        
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
