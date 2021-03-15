using Eto.Forms;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Settings.Sections.Recording.CodecsOptions.VideoOptions
{
    /// <summary>
    /// MPEG-4 options dialog.
    /// </summary>
    internal class MpegCodecOptions : CodecOptionsBase
    {
        private const string About = "The range of the quality level is 1–31, where 1 is highest quality/largest filesize and 31 is the lowest quality/smallest filesize";
        private NumericStepper mpeg4Quality;

        /// <summary>
        /// Initializes MPEG-4 options dialog.
        /// </summary>
        public MpegCodecOptions(FFmpegOptions options) : base(options, "MPEG-4 options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            mpeg4Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 31,
                MinValue = 1,
                Increment = 1
            };

            AddQualityRow("Quality", mpeg4Quality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            mpeg4Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.XviDQscale);
        }
    }
}
