using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Recording.Ffmpeg.Encoding;

namespace RedShot.Recording.Settings.CodecsOptions.VideoOptions
{
    /// <summary>
    /// H264 and H265 options dialog.
    /// </summary>
    internal class H264H265CodecOptions : CodecOptionsBase
    {
        private const string About = "The range of the CRF scale is 0–51, where 0 is lossless, 23 is the default, and 51 is worst quality possible.\n" +
                "A lower value generally leads to higher quality, and a subjectively sane range is 17–28.\n" +
                "Consider 17 or 18 to be visually lossless or nearly so;\n" +
                "it should look the same or nearly the same as the input but it isn't technically lossless.";

        private ComboBox x264Preset;
        private NumericStepper x264Quality;

        /// <summary>
        /// Initializes H264/H265 options dialog.
        /// </summary>
        public H264H265CodecOptions(FFmpegOptions options) : base(options, "H264 / H265 options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            x264Preset = new ComboBox()
            {
                ReadOnly = true
            };

            x264Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 51,
                MinValue = 0,
                Increment = 1
            };

            AddQualityRow("Preset", x264Preset);
            AddQualityRow("Quality", x264Quality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            x264Preset.BindWithEnum<FFmpegX264Preset>().BindDataContext((FFmpegOptions o) => o.X264Preset);

            x264Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.X264Crf);
        }
    }
}
