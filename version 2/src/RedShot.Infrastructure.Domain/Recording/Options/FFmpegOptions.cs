using RedShot.Infrastructure.Domain.Recording.Devices;
using System;

namespace RedShot.Infrastructure.Domain.Recording.Options
{
    /// <summary>
    /// FFmpeg options.
    /// </summary>
    public class FFmpegOptions : ICloneable
    {
        /// <summary>
        /// FPS.
        /// </summary>
        public int Fps { get; set; }

        /// <summary>
        /// Draw cursor.
        /// </summary>
        public bool DrawCursor { get; set; }

        /// <summary>
        /// Use GDI grab.
        /// Windows only.
        /// </summary>
        public bool UseGdigrab { get; set; }

        /// <summary>
        /// Video device.
        /// </summary>
        public Device VideoDevice { get; set; }

        /// <summary>
        /// Video codec.
        /// </summary>
        public FFmpegVideoCodec VideoCodec { get; set; }

        /// <summary>
        /// Audio codec.
        /// </summary>
        public FFmpegAudioCodec AudioCodec { get; set; }

        /// <summary>
        /// User's custom arguments.
        /// </summary>
        public string UserArgs { get; set; }

        /// <summary>
        /// X264 preset.
        /// </summary>
        public FFmpegX264Preset X264Preset { get; set; }

        /// <summary>
        /// VP9 CRF.
        /// </summary>
        public int Vp9Crf { get; set; }

        /// <summary>
        /// X264 CRF.
        /// </summary>
        public int X264Crf { get; set; }

        /// <summary>
        /// XviD quality scale.
        /// </summary>
        public int XviDQscale { get; set; }

        /// <summary>
        /// VP9 bit rate.
        /// </summary>
        public int Vp9Bitrate { get; set; }

        /// <summary>
        /// AAC quality scale.
        /// </summary>
        public int AacQScale { get; set; }

        /// <summary>
        /// Opus bit rate.
        /// </summary>
        public int OpusBitrate { get; set; }

        /// <summary>
        /// Vorbis quality scale.
        /// </summary>
        public int VorbisQscale { get; set; }

        /// <summary>
        /// MP3 quality scale.
        /// </summary>
        public int MP3Qscale { get; set; }

        /// <summary>
        /// Video extension.
        /// </summary>
        public string Extension => VideoCodec switch
        {
            FFmpegVideoCodec.Libx264 or FFmpegVideoCodec.Libx265 => "mp4",
            FFmpegVideoCodec.Libvpx_vp9 => "webm",
            FFmpegVideoCodec.Libxvid => "avi",
            _ => "mp4",
        };

        /// <summary>
        /// Clone.
        /// </summary>
        public FFmpegOptions Clone()
        {
            return MemberwiseClone() as FFmpegOptions;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
