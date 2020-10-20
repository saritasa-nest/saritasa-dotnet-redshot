using System.ComponentModel;

namespace RedShot.Infrastructure.Recording.Ffmpeg.Encoding
{
    /// <summary>
    /// FFmpeg x264 presets.
    /// </summary>
    public enum FFmpegX264Preset
    {
        [Description("Ultra fast")]
        Ultrafast,
        [Description("Super fast")]
        Superfast,
        [Description("Very fast")]
        Veryfast,
        [Description("Faster")]
        Faster,
        [Description("Fast")]
        Fast,
        [Description("Medium")]
        Medium,
        [Description("Slow")]
        Slow,
        [Description("Slower")]
        Slower,
        [Description("Very slow")]
        Veryslow
    }
}
