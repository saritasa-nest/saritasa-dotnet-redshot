using RedShot.Helpers.Ffmpeg.Options;
using RedShot.Helpers.FtpModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedShot.Configuration
{
    /// <summary>
    /// Configuration object.
    /// </summary>
    public sealed class YamlConfig : ICloneable
    {
        /// <summary>
        /// Ftp accounts.
        /// </summary>
        public List<FtpAccount> FtpAccounts { get; internal set; } = new List<FtpAccount>();

        public FFmpegOptions FFmpegOptions { get; set; } = new FFmpegOptions();

        /// <summary>
        /// Some extensions if need.
        /// </summary>
        public Dictionary<string, object> Extensions { get; internal set; } = new Dictionary<string, object>();

        public YamlConfig Clone()
        {
            var clone = new YamlConfig();

            clone.FFmpegOptions = FFmpegOptions;

            clone.FtpAccounts.AddRange(FtpAccounts.Select(a => a.Clone()));

            foreach (var extension in Extensions)
            {
                if (extension.Value is ICloneable cloneable)
                {
                    clone.Extensions.Add(extension.Key, cloneable.Clone());
                }
                else
                {
                    clone.Extensions.Add(extension.Key, extension.Value);
                }
            }

            return clone;

        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
