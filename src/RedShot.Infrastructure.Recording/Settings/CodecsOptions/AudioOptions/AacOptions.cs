using Eto.Forms;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// AAC options dialog.
    /// </summary>
    internal class AacOptions : CodecOptionsBase
    {
        private const string About = "The range of the quality is 0-9 where a lower value is a higher quality. " +
            "0-3 will normally produce transparent results, 4 (default) should be close to perceptual transparency, and 6 produces an acceptable quality.";

        private NumericStepper codecQuality;

        /// <summary>
        /// Initializes AAC options dialog.
        /// </summary>
        public AacOptions(FFmpegOptions options) : base(options, "AAC options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            codecQuality = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 9,
                Increment = 1
            };

            AddQualityRow("Quality", codecQuality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.AacQScale);
        }
    }
}
