﻿using System.ComponentModel;

namespace RedShot.Recording.Helpers
{
    public enum FFmpegAudioCodec
    {
        [Description("None")]
        none,
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
