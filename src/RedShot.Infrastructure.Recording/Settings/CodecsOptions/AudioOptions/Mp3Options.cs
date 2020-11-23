using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// MP3 options dialog.
    /// </summary>
    internal class Mp3Options : CodecOptionsBase
    {
        private const string About = "The range of the quality is 1-5 where 1 is lowest quality and 5 is highest quality.";

        private NumericStepper codecQuality;

        /// <summary>
        /// Initializes MP3 options dialog.
        /// </summary>
        public Mp3Options(FFmpegOptions options) : base(options, "Mp3 options", About)
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

            AddQualityRow("Quality", codecQuality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.MP3Qscale);
        }
    }
}
