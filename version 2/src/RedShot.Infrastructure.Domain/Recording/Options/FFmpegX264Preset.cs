using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RedShot.Infrastructure.Domain.Recording.Options
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
