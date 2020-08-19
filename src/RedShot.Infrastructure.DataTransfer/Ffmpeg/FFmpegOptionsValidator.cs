using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;

namespace RedShot.Infrastructure.DataTransfer.Ffmpeg
{
    public static class FFmpegOptionsValidator
    {
        public static FFmpegValidationResult Validate(this FFmpegOptions options)
        {
            var result = new FFmpegValidationResult();

            switch (options.VideoCodec)
            {
                case FFmpegVideoCodec.libx264:
                case FFmpegVideoCodec.libxvid:
                    if (options.AudioCodec == FFmpegAudioCodec.libopus || options.AudioCodec == FFmpegAudioCodec.libvorbis)
                    {
                        return GetIncompatibleCodecResult(options);
                    }
                    break;

                case FFmpegVideoCodec.libvpx_vp9:
                    if (options.AudioCodec == FFmpegAudioCodec.libmp3lame || options.AudioCodec == FFmpegAudioCodec.libvoaacenc)
                    {
                        return GetIncompatibleCodecResult(options);
                    }
                    break;
            }

            result.IsSuccess = true;

            return result;
        }

        private static FFmpegValidationResult GetIncompatibleCodecResult(FFmpegOptions options)
        {
            var result = new FFmpegValidationResult();

            result.IsSuccess = false;
            result.Errors.Add(GetIncompatibleCodecMessage(options));

            return result;
        }

        private static string GetIncompatibleCodecMessage(FFmpegOptions options)
        {
            return $"You can't use {EnumDescription<FFmpegAudioCodec>.GetDescriptionName(options.AudioCodec)} audio codec with " +
                            $"{EnumDescription<FFmpegVideoCodec>.GetDescriptionName(options.VideoCodec)} video codec";
        }
    }
}
