using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Recording.Ffmpeg.Encoding;

namespace RedShot.Infrastructure.Recording.Validation
{
    /// <summary>
    /// Validates FFmpeg options.
    /// </summary>
    public static class FFmpegOptionsValidator
    {
        /// <summary>
        /// Returns result of the validation.
        /// Extension for FFmpeg options.
        /// </summary>
        public static ValidationResult Validate(this FFmpegOptions options)
        {
            switch (options.VideoCodec)
            {
                case FFmpegVideoCodec.Libx264:
                case FFmpegVideoCodec.Libxvid:
                    if (options.AudioCodec == FFmpegAudioCodec.Libopus || options.AudioCodec == FFmpegAudioCodec.Libvorbis)
                    {
                        return GetIncompatibleCodecResult(options);
                    }
                    break;

                case FFmpegVideoCodec.Libvpx_vp9:
                    if (options.AudioCodec == FFmpegAudioCodec.Libmp3lame || options.AudioCodec == FFmpegAudioCodec.Libvoaacenc)
                    {
                        return GetIncompatibleCodecResult(options);
                    }
                    break;
            }

            return new ValidationResult(true);
        }

        private static ValidationResult GetIncompatibleCodecResult(FFmpegOptions options)
        {
            return new ValidationResult(false, GetIncompatibleCodecMessage(options));
        }

        private static string GetIncompatibleCodecMessage(FFmpegOptions options)
        {
            return $"You can't use {EnumDescription<FFmpegAudioCodec>.GetDescriptionName(options.AudioCodec)} audio codec with " +
                            $"{EnumDescription<FFmpegVideoCodec>.GetDescriptionName(options.VideoCodec)} video codec";
        }
    }
}
