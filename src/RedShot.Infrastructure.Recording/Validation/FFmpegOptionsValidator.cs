using System.Collections.Generic;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;

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
