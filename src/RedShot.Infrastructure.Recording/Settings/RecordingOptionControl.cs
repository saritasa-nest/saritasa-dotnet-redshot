using System;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg;
using RedShot.Infrastructure.Recording.Ffmpeg.Encoding;
using RedShot.Recording.Settings.CodecsOptions.AudioOptions;
using RedShot.Recording.Settings.CodecsOptions.VideoOptions;

namespace RedShot.Infrastructure.Recording.Settings
{
    /// <summary>
    /// Recording option dialog.
    /// </summary>
    internal partial class RecordingOptionControl : Panel
    {
        private FFmpegConfiguration ffmpegConfiguration;
        private ComboBox videoCodec;
        private Button videoCodecOptionsButton;
        private ComboBox audioCodec;
        private Button audioCodecOptionsButton;
        private Button setDefaultButton;
        private NumericStepper fps;
        private TextBox userArgs;
        private CheckBox showCursor;
        private CheckBox useGdigrab;

        /// <summary>
        /// Initializes recording option dialog.
        /// </summary>
        public RecordingOptionControl(FFmpegConfiguration ffmpegConfiguration)
        {
            this.ffmpegConfiguration = ffmpegConfiguration;

            InitializeComponents();
        }

        private void SetDefaultButtonClicked(object sender, EventArgs e)
        {
            Content.Unbind();
            ffmpegConfiguration.FFmpegOptions = new FFmpegOptions();
            BindOptions();
        }

        private void BindOptions()
        {
            Content.DataContext = ffmpegConfiguration.FFmpegOptions;

            fps.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.Fps);
            userArgs.TextBinding.BindDataContext((FFmpegOptions o) => o.UserArgs);
            var gdigrabBind = useGdigrab.CheckedBinding.BindDataContext((FFmpegOptions o) => o.UseGdigrab);

            showCursor.CheckedBinding.BindDataContext((FFmpegOptions o) => o.DrawCursor);

            audioCodec.BindWithEnum<FFmpegAudioCodec>().BindDataContext((FFmpegOptions o) => o.AudioCodec);

            videoCodec.BindWithEnum<FFmpegVideoCodec>().BindDataContext((FFmpegOptions o) => o.VideoCodec);
        }

        private void VideoCodecOptionsButtonClicked(object sender, EventArgs e)
        {
            Dialog options;
            switch (ffmpegConfiguration.FFmpegOptions.VideoCodec)
            {
                case FFmpegVideoCodec.Libx264:
                case FFmpegVideoCodec.Libx265:
                    options = new H264H265CodecOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                case FFmpegVideoCodec.Libvpx_vp9:
                    options = new Vp9CodecOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                case FFmpegVideoCodec.Libxvid:
                    options = new MpegCodecOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }

        private void AudioCodecOptionsButtonClicked(object sender, EventArgs e)
        {
            Dialog options;
            switch (ffmpegConfiguration.FFmpegOptions.AudioCodec)
            {
                case FFmpegAudioCodec.Libvoaacenc:
                    options = new AacOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libopus:
                    options = new OpusOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libvorbis:
                    options = new VorbisOptions(ffmpegConfiguration.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libmp3lame:
                    options = new Mp3Options(ffmpegConfiguration.FFmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }
    }
}
