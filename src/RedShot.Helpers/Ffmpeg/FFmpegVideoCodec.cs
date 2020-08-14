using System.ComponentModel;

namespace RedShot.Helpers.Ffmpeg
{
    public enum FFmpegVideoCodec
    {
        [Description("H.264 / x264")]
        libx264,
        [Description("VP9 (WebM)")]
        libvpx_vp9,
        [Description("MPEG-4 / Xvid")]
        libxvid,
        [Description("H.264 / NVENC")]
        h264_nvenc,
        [Description("H.264 / Quick Sync")]
        h264_qsv
    }
}
