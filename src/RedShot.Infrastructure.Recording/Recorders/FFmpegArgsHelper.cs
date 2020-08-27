using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;
using System.Text;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// FFmpeg args helper.
    /// </summary>
    public static class FFmpegArgsHelper
    {
        /// <summary>
        /// Returns parsed string from FFmpeg options.
        /// </summary>
        public static string GetFFmpegArgs(this FFmpegOptions options)
        {
            var args = new StringBuilder();

            args.Append("-rtbufsize 150M ");

            if (!string.IsNullOrEmpty(options.UserArgs))
            {
                args.Append(options.UserArgs + " ");
            }

            switch (options.VideoCodec)
            {
                case FFmpegVideoCodec.libx264: // https://trac.ffmpeg.org/wiki/Encode/H.264
                    args.Append($"-c:v libx264 ");
                    args.AppendFormat("-preset {0} ", options.X264Preset);
                    args.AppendFormat("-crf {0} ", options.X264Crf);
                    args.AppendFormat("-pix_fmt {0} ", "yuv420p");
                    args.AppendFormat("-movflags {0} ", "+faststart");
                    break;
                case FFmpegVideoCodec.libx265: // https://trac.ffmpeg.org/wiki/Encode/H.265
                    args.Append($"-c:v libx265 ");
                    args.AppendFormat("-preset {0} ", options.X264Preset);
                    args.AppendFormat("-crf {0} ", options.X264Crf);
                    break;
                case FFmpegVideoCodec.libvpx_vp9: // https://trac.ffmpeg.org/wiki/Encode/VP9
                    args.Append($"-c:v libvpx-vp9 ");
                    args.AppendFormat("-crf {0} ", options.Vp9Crf);
                    args.AppendFormat("-b:v {0}k ", options.Vp9Bitrate);
                    args.AppendFormat("-pix_fmt {0} ", "yuv420p");
                    break;
                case FFmpegVideoCodec.libxvid: // https://trac.ffmpeg.org/wiki/Encode/MPEG-4
                    args.Append($"-c:v libxvid ");
                    args.AppendFormat("-qscale:v {0} ", options.XviDQscale);
                    break;
            }

            switch (options.AudioCodec)
            {
                case FFmpegAudioCodec.libvoaacenc: // http://trac.ffmpeg.org/wiki/Encode/AAC
                    args.AppendFormat("-c:a aac -ac 2 -vbr {0} ", options.AacQScale);
                    break;
                case FFmpegAudioCodec.libopus: // https://www.ffmpeg.org/ffmpeg-codecs.html#libopus-1
                    args.AppendFormat("-c:a libopus -b:a {0}k ", options.OpusBitrate);
                    break;
                case FFmpegAudioCodec.libvorbis: // http://trac.ffmpeg.org/wiki/TheoraVorbisEncodingGuide
                    args.AppendFormat("-c:a libvorbis -qscale:v {0} ", options.VorbisQscale);
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

            args.Append("-y ");
            args.AppendFormat("\"{0}\"", filepath);

            return args.ToString();
        }
    }
}
