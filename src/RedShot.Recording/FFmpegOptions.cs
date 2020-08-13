using RedShot.Recording.Devices;
using RedShot.Recording.Helpers;

namespace RedShot.Recording
{
    public class FFmpegOptions
    {
        public FFmpegOptions()
        {
            Fps = 30;
            DrawCursor = true;
            VideoCodec = FFmpegVideoCodec.libx264;
            AudioCodec = FFmpegAudioCodec.libvoaacenc;
            X264Preset = FFmpegX264Preset.faster;
            X264Crf = 28;
            XviDQscale = 10;
            Bitrate = 3000;
            NVENCPreset = FFmpegNVENCPreset.llhp;
            QSVPreset = FFmpegQSVPreset.fast;
            AACBitrate = 128;
            OpusBitrate = 128;
            VorbisQscale = 3;
            MP3Qscale = 4;
            UserArgs = string.Empty;
        }

        public int Fps { get; set; }

        public bool DrawCursor { get; set; }

        // General
        public Device VideoDevice { get; set; }

        public Device AudioDevice { get; set; }

        public FFmpegVideoCodec VideoCodec { get; set; }

        public FFmpegAudioCodec AudioCodec { get; set; }

        public string UserArgs { get; set; }

        // Video
        public FFmpegX264Preset X264Preset { get; set; }

        public int X264Crf { get; set; }

        public int XviDQscale { get; set; }

        public int Bitrate { get; set; }

        public FFmpegNVENCPreset NVENCPreset { get; set; }

        public FFmpegQSVPreset QSVPreset { get; set; }

        // Audio
        public int AACBitrate { get; set; }

        public int OpusBitrate { get; set; }

        public int VorbisQscale { get; set; }

        public int MP3Qscale { get; set; }

        public string Extension
        {
            get
            {
                switch (VideoCodec)
                {
                    case FFmpegVideoCodec.libx264:
                    case FFmpegVideoCodec.h264_qsv:
                    case FFmpegVideoCodec.h264_nvenc:
                        return "mp4";
                    case FFmpegVideoCodec.libvpx_vp9:
                        return "webm";
                    case FFmpegVideoCodec.libxvid:
                        return "avi";
                }

                return "mp4";
            }
        }
    }
}
