using RedShot.Infrastructure.Abstractions;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using System;

namespace RedShot.Infrastructure.Configuration.Options
{
    public class FFmpegConfiguration : IConfigurationOption
    {
        public FFmpegConfiguration()
        {
            UniqueName = "FFmpeg configuration";
            Options = new FFmpegOptions();
        }

        public string UniqueName { get; }

        public FFmpegOptions Options { get; set; }

        public FFmpegConfiguration Clone()
        {
            var clone = (FFmpegConfiguration)MemberwiseClone();
            clone.Options = Options.Clone();

            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public IConfigurationOption DecodeSection(IEncryptionService encryptionService)
        {
            return this;
        }

        public IConfigurationOption EncodeSection(IEncryptionService encryptionService)
        {
            return this;
        }
    }
}
