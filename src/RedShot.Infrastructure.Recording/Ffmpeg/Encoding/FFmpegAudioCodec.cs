using System.ComponentModel;

namespace RedShot.Infrastructure.Recording.Ffmpeg.Encoding
{
    public enum FFmpegAudioCodec
    {
        [Description("AAC")]
        Libvoaacenc,
        [Description("Opus")]
        Libopus,
        [Description("Vorbis")]
        Libvorbis,
        [Description("MP3")]
        Libmp3lame
    }
}
