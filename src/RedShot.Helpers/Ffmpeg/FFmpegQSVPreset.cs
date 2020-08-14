using System.ComponentModel;

namespace RedShot.Helpers.Ffmpeg
{
    public enum FFmpegQSVPreset
    {
        [Description("Very fast")]
        veryfast,
        [Description("Faster")]
        faster,
        [Description("Fast")]
        fast,
        [Description("Medium")]
        medium,
        [Description("Slow")]
        slow,
        [Description("Slower")]
        slower,
        [Description("Very slow")]
        veryslow
    }
}
