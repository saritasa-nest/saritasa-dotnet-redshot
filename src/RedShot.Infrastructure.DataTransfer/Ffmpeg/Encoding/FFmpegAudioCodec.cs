using System.ComponentModel;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding
{
    public enum FFmpegAudioCodec
    {
        [Description("AAC")]
        libvoaacenc,
        [Description("Opus")]
        libopus,
        [Description("Vorbis")]
        libvorbis,
        [Description("MP3")]
        libmp3lame
    }
}
