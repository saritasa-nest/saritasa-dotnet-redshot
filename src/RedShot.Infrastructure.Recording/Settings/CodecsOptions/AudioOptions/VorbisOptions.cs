using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// Vorbis options view.
    /// </summary>
    internal class VorbisOptions : AudioCodecOptionsBase<NumericStepper>
    {
        private static readonly string about = "Range is 0–10, where 10 is highest quality. 5–7 is a good range to try.";

        /// <inheritdoc/>
        public VorbisOptions(FFmpegOptions options) : base(options, "Vorbis options", about)
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

            base.InitializeComponents();
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.VorbisQscale);
        }
    }
}