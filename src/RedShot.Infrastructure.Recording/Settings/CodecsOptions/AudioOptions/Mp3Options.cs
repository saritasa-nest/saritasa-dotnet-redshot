using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// MP3 options view.
    /// </summary>
    internal class Mp3Options : AudioCodecOptionsBase<NumericStepper>
    {
        private static readonly string about = "The range of the quality is 1-5 where 1 is lowest quality and 5 is highest quality.";

        /// <inheritdoc/>
        public Mp3Options(FFmpegOptions options) : base(options, "Mp3 options", about)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            codecQuality = new NumericStepper()
            {
                MinValue = 1,
                MaxValue = 5,
                Increment = 1
            };

            base.InitializeComponents();
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.MP3Qscale);
        }
    }
}
