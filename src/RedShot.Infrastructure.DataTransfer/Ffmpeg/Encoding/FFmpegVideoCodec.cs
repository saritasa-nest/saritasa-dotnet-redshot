using System.ComponentModel;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding
{
    public enum FFmpegVideoCodec
    {
        [Description("H.264 / x264")]
        libx264,
        [Description("H.265 / HEVC")]
        libx265,
        [Description("VP9 (WebM)")]
        libvpx_vp9,
        [Description("MPEG-4 / Xvid")]
        libxvid,
    }
}
