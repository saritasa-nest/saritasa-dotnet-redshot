using Eto.Forms;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Settings.Sections.Recording.CodecsOptions.VideoOptions
{
    /// <summary>
    /// VP9 options dialog.
    /// </summary>
    internal class Vp9CodecOptions : CodecOptionsBase
    {
        private const string About = "The quality value can be from 0–63. Lower values mean better quality. " +
                "Recommended values range from 15–35, with 31 being recommended for 1080p HD video";

        private NumericStepper vp9Quality;
        private TextBox vp9Bitrate;

        /// <summary>
        /// Initializes VP9 options dialog.
        /// </summary>
        public Vp9CodecOptions(FFmpegOptions options) : base(options, "VP9 codec options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            vp9Bitrate = new TextBox();
            vp9Quality = new NumericStepper()
            {
                Height = 21,
                MaxValue = 63,
                MinValue = 0,
                Increment = 1
            };

            AddQualityRow("Bitrate", CreateBitrateQualityRow());
            AddQualityRow("Quality", vp9Quality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            vp9Bitrate.TextBinding.Convert(
                l =>
                {
                    if (int.TryParse(l, out int result))
                    {
                        return result;
                    }
                    else
                    {
                        return 20;
                    }
                },
                v =>
                {
                    return v.ToString();
                }).BindDataContext((FFmpegOptions o) => o.Vp9Bitrate);

            vp9Quality.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.Vp9Crf);
        }

        private Control CreateBitrateQualityRow()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    vp9Bitrate,
                    new Label()
                    {
                        Text = "K"
                    }
                }
            };
        }
    }
}
