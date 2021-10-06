namespace RedShot.Infrastructure.Configuration.Models.Recording
{
    /// <summary>
    /// Recording configuration.
    /// </summary>
    public class RecordingConfiguration
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordingConfiguration()
        {
            AudioData = new AudioData();
            FFmpegData = new FFmpegData();
        }

        /// <summary>
        /// Audio data.
        /// </summary>
        public AudioData AudioData { get; set; }

        /// <summary>
        /// FFmpeg data.
        /// </summary>
        public FFmpegData FFmpegData { get; set; }
    }
}
