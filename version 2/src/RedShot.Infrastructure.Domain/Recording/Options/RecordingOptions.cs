using RedShot.Infrastructure.Domain.Recording.Options;

namespace RedShot.Infrastructure.Recording.Common.Ffmpeg
{
    /// <summary>
    /// Recording options.
    /// </summary>
    public class RecordingOptions
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public RecordingOptions()
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
