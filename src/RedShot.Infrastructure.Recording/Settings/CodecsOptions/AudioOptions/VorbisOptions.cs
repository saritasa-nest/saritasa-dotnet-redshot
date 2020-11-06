using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// Vorbis options dialog.
    /// </summary>
    internal class VorbisOptions : CodecOptionsBase
    {
        private const string About = "Range is 0–10, where 10 is highest quality. 5–7 is a good range to try.";

        private NumericStepper codecQuality;

        /// <summary>
        /// Initializes Vorbis options dialog.
        /// </summary>
        public VorbisOptions(FFmpegOptions options) : base(options, "Vorbis options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            codecQuality = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 10,
                Increment = 1
            };

            AddQualityRow("Quality", codecQuality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.VorbisQscale);
        }
    }
}