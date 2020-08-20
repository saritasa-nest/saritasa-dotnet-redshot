using Eto.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;

namespace RedShot.Recording.Views.CodecsOptions.AudioOptions
{
    internal class AacOptions : AudioCodecOptionsBase<NumericStepper>
    {
        private static readonly string about = "The range of the quality is 0-9 where a lower value is a higher quality. " +
            "0-3 will normally produce transparent results, 4 (default) should be close to perceptual transparency, and 6 produces an acceptable quality.";

        public AacOptions(FFmpegOptions options) : base(options, "AAC options", about)
        {
        }

        protected override void InitializeComponents()
        {
            codecQuality = new NumericStepper()
            {
                MinValue = 0,
                MaxValue = 9,
                Increment = 1
            };

            base.InitializeComponents();
        }

        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.AacQScale);
        }
    }
}
