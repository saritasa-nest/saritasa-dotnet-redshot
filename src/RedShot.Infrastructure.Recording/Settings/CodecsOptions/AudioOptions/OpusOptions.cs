using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// Opus options dialog.
    /// </summary>
    internal class OpusOptions : CodecOptionsBase
    {
        private const string About = "Set the bit rate in kbit/s.";
        private TextBox bitrate;

        /// <summary>
        /// Initializes Opus options dialog.
        /// </summary>
        public OpusOptions(FFmpegOptions options) : base(options, "Opus options", About)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            bitrate = new TextBox();

            var codecQuality = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    bitrate,
                    new Label()
                    {
                        Text = "K"
                    }
                }
            };

            AddQualityRow("Bitrate", codecQuality);
        }

        /// <inheritdoc/>
        protected override void Bind()
        {
            Content.DataContext = options;

            bitrate.TextBinding.Convert(
                l =>
                {
                    if (int.TryParse(l, out int result))
                    {
                        return result;
                    }
                    else
                    {
                        return 128;
                    }
                },
                v =>
                {
                    return v.ToString();
                }).BindDataContext((FFmpegOptions o) => o.OpusBitrate);
        }
    }
}
