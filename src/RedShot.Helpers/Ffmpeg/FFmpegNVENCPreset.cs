using System.ComponentModel;

namespace RedShot.Helpers.Ffmpeg
{
    public enum FFmpegNVENCPreset
    {
        [Description("Default")]
        @default,
        [Description("High quality 2 passes")]
        slow,
        [Description("High quality 1 pass")]
        medium,
        [Description("High performance 1 pass")]
        fast,
        [Description("High performance")]
        hp,
        [Description("High quality")]
        hq,
        [Description("Bluray disk")]
        bd,
        [Description("Low latency")]
        ll,
        [Description("Low latency high quality")]
        llhq,
        [Description("Low latency high performance")]
        llhp,
        [Description("Lossless")]
        lossless,
        [Description("Lossless high performance")]
        losslesshp
    }
}
