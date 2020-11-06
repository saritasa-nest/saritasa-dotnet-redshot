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
        private FFmpegOptions ffmpegOptions;
        private ComboBox videoCodec;
        private Button videoCodecOptionsButton;
        private ComboBox audioCodec;
        private Button audioCodecOptionsButton;
        private DefaultButton setDefaultButton;
        private NumericStepper fps;
        private TextBox userArgs;
        private CheckBox showCursor;
        private CheckBox useGdigrab;

        /// <summary>
        /// Initializes recording option dialog.
        /// </summary>
        public RecordingOptionControl(FFmpegOptions ffmpegOptions)
        {
            this.ffmpegOptions = ffmpegOptions;

            InitializeComponents();
        }

        private void SetDefaultButton_Clicked(object sender, EventArgs e)
        {
            Content.Unbind();
            ffmpegOptions = new FFmpegOptions();
            BindOptions();
        }

        private void BindOptions()
        {
            Content.DataContext = ffmpegOptions;

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
            switch (ffmpegOptions.VideoCodec)
            {
                case FFmpegVideoCodec.Libx264:
                case FFmpegVideoCodec.Libx265:
                    options = new H264H265CodecOptions(ffmpegOptions);
                    break;

                case FFmpegVideoCodec.Libvpx_vp9:
                    options = new Vp9CodecOptions(ffmpegOptions);
                    break;

                case FFmpegVideoCodec.Libxvid:
                    options = new MpegCodecOptions(ffmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }

        private void AudioCodecOptionsButtonClicked(object sender, EventArgs e)
        {
            Dialog options;
            switch (ffmpegOptions.AudioCodec)
            {
                case FFmpegAudioCodec.Libvoaacenc:
                    options = new AacOptions(ffmpegOptions);
                    break;

                case FFmpegAudioCodec.Libopus:
                    options = new OpusOptions(ffmpegOptions);
                    break;

                case FFmpegAudioCodec.Libvorbis:
                    options = new VorbisOptions(ffmpegOptions);
                    break;

                case FFmpegAudioCodec.Libmp3lame:
                    options = new Mp3Options(ffmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }
    }
}
