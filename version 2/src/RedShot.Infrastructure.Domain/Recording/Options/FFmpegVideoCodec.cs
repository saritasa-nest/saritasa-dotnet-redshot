using System.ComponentModel;

namespace RedShot.Infrastructure.Domain.Recording.Options
{
    /// <summary>
    /// FFmpeg video codecs.
    /// </summary>
    public enum FFmpegVideoCodec
    {
        [Description("H.264 / x264")]
        Libx264,
        [Description("H.265 / HEVC")]
        Libx265,
        [Description("VP9 (WebM)")]
        Libvpx_vp9,
        [Description("MPEG-4 / Xvid")]
        Libxvid,
    }
}
