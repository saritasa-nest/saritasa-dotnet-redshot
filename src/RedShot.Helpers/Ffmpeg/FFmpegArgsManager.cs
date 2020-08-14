using System.Text;

namespace RedShot.Helpers.Ffmpeg
{
    public static class FFmpegArgsManager
    {
        public static string GetFFmpegArgsFromOptions(FFmpegOptions options)
        {
            StringBuilder args = new StringBuilder();
            args.Append("-rtbufsize 150M "); // default real time buffer size was 3041280 (3M)

            if (!string.IsNullOrEmpty(options.UserArgs))
            {
                args.Append(options.UserArgs + " ");
            }

            switch (options.VideoCodec)
            {
                case FFmpegVideoCodec.libx264: // https://trac.ffmpeg.org/wiki/Encode/H.264
                    args.AppendFormat("-preset {0} ", options.X264Preset);
                    args.AppendFormat("-crf {0} ", options.X264Crf);
                    args.AppendFormat("-pix_fmt {0} ", "yuv420p"); // -pix_fmt yuv420p required otherwise can't stream in Chrome
                    args.AppendFormat("-movflags {0} ", "+faststart"); // This will move some information to the beginning of your file and allow the video to begin playing before it is completely downloaded by the viewer
                    break;
                case FFmpegVideoCodec.libvpx_vp9: // https://trac.ffmpeg.org/wiki/Encode/VP9
                    args.AppendFormat("-b:v {0}k ", options.Bitrate);
                    args.AppendFormat("-pix_fmt {0} ", "yuv420p"); // -pix_fmt yuv420p required otherwise causing issues in Chrome related to WebM transparency support
                    break;
                case FFmpegVideoCodec.libxvid: // https://trac.ffmpeg.org/wiki/Encode/MPEG-4
                    args.AppendFormat("-qscale:v {0} ", options.XviDQscale);
                    break;
                case FFmpegVideoCodec.h264_nvenc: // https://trac.ffmpeg.org/wiki/HWAccelIntro#NVENC
                    args.AppendFormat("-preset {0} ", options.NVENCPreset);
                    args.AppendFormat("-b:v {0}k ", options.Bitrate);
                    args.AppendFormat("-pix_fmt {0} ", "yuv420p");
                    break;

                case FFmpegVideoCodec.h264_qsv: // https://trac.ffmpeg.org/wiki/Hardware/QuickSync
                    args.AppendFormat("-preset {0} ", options.QSVPreset);
                    args.AppendFormat("-b:v {0}k ", options.Bitrate);
                    break;
            }

            switch (options.AudioCodec)
            {
                case FFmpegAudioCodec.libvoaacenc: // http://trac.ffmpeg.org/wiki/Encode/AAC
                    args.AppendFormat("-c:a aac -ac 2 -b:a {0}k ", options.AACBitrate); // -ac 2 required otherwise failing with 7.1
                    break;
                case FFmpegAudioCodec.libopus: // https://www.ffmpeg.org/ffmpeg-codecs.html#libopus-1
                    args.AppendFormat("-c:a libopus -b:a {0}k ", options.OpusBitrate);
                    break;
                case FFmpegAudioCodec.libvorbis: // http://trac.ffmpeg.org/wiki/TheoraVorbisEncodingGuide
                    args.AppendFormat("-c:a libvorbis -qscale:a {0} ", options.VorbisQscale);
                    break;
                case FFmpegAudioCodec.libmp3lame: // http://trac.ffmpeg.org/wiki/Encode/MP3
                    args.AppendFormat("-c:a libmp3lame -qscale:a {0} ", options.MP3Qscale);
                    break;
            }

            return args.ToString();
        }

        public static string GetArgsForOutput(string filepath)
        {
            var args = new StringBuilder();

            args.Append("-y "); // overwrite file
            args.AppendFormat("\"{0}\"", filepath);

            return args.ToString();
        }
    }
}
