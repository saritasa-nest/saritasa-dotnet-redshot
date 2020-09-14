using Eto.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;

namespace RedShot.Recording.Settings.CodecsOptions.AudioOptions
{
    /// <summary>
    /// Opus options view.
    /// </summary>
    internal class OpusOptions : AudioCodecOptionsBase<Control>
    {
        private TextBox bitrate;

        private static readonly string about = "Set the bit rate in kilobits/s.";

        /// <inheritdoc/>
        public OpusOptions(FFmpegOptions options) : base(options, "Opus options", about)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeComponents()
        {
            bitrate = new TextBox()
            {
                Size = new Eto.Drawing.Size(70, 21)
            };

            codecQuality = new StackLayout()
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

            base.InitializeComponents();
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
