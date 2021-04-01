using RedShot.Infrastructure.Common.Recording.Encoding;

namespace RedShot.Infrastructure.Configuration.Models.Recording
{
    /// <summary>
    /// Contains data about FFmpeg parameters.
    /// </summary>
    public class FFmpegData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public FFmpegData()
        {
            Fps = 30;
            DrawCursor = true;
            VideoCodec = FFmpegVideoCodec.Libx264;
            AudioCodec = FFmpegAudioCodec.Libvoaacenc;
            X264Preset = FFmpegX264Preset.Faster;
            X264Crf = 23;
            Vp9Crf = 35;
            XviDQscale = 10;
            Vp9Bitrate = 3000;
            AacQScale = 3;
            OpusBitrate = 128;
            VorbisQscale = 3;
            MP3Qscale = 4;
            UseGdigrab = true;
        }

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
        public DeviceData VideoDevice { get; set; }

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
    }
}
