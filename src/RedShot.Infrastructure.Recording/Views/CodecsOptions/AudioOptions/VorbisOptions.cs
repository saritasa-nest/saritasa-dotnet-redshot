using Eto.Forms;
using RedShot.Helpers.Ffmpeg.Options;

namespace RedShot.Recording.Views.CodecsOptions.AudioOptions
{
    internal class VorbisOptions : AudioCodecOptionsBase<NumericStepper>
    {
        private static readonly string about = "Range is 0–10, where 10 is highest quality. 5–7 is a good range to try.";

        public VorbisOptions(FFmpegOptions options) : base(options, "Vorbis options", about)
        {
        }

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

        protected override void Bind()
        {
            Content.DataContext = options;

            codecQuality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.VorbisQscale);
        }
    }
}