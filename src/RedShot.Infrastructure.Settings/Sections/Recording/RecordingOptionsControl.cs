using System;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;
using RedShot.Infrastructure.Common.Recording.Encoding;
using RedShot.Infrastructure.Settings.Sections.Recording.CodecsOptions.AudioOptions;
using RedShot.Infrastructure.Settings.Sections.Recording.CodecsOptions.VideoOptions;

namespace RedShot.Infrastructure.Settings.Sections.Recording
{
    /// <summary>
    /// Recording option dialog.
    /// </summary>
    internal partial class RecordingOptionsControl : Panel
    {
        /// <summary>
        /// Check FFmpeg binaries interval in seconds.
        /// </summary>
        private const int CheckFfmpegInterval = 2;

        private readonly UITimer ffmpegCheckTimer;
        private readonly RecordingOptions recordingOptions;
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
        /// Active configuration data.
        /// </summary>
        public RecordingOptions RecordingOptions => recordingOptions;

        /// <summary>
        /// Initializes recording option dialog.
        /// </summary>
        public RecordingOptionsControl(RecordingOptions recordingOptions)
        {
            this.recordingOptions = recordingOptions;

            if (!RecordingManager.Instance.RecordingService.CheckFFmpeg())
            {
                var control = new FfmpegUninstalledControl();
                ffmpegCheckTimer = new UITimer()
                {
                    Interval = CheckFfmpegInterval
                };
                ffmpegCheckTimer.Elapsed += CheckTimerElapsed;
                ffmpegCheckTimer.Start();
                Content = control;
            }
            else
            {
                InitializeComponents();
            }
        }

        /// <summary>
        /// If FFmpeg is not installed on the PC, the FFmpeg options will not be displayed.
        /// Instead, the control shows a text in the settings “FFmpeg is not installed” and a button below it “Install”.
        /// If FFmpeg binaries were found the control initializes FFmpeg options.
        /// </summary>
        private void CheckTimerElapsed(object sender, EventArgs e)
        {
            ffmpegCheckTimer.Stop();

            if (RecordingManager.Instance.RecordingService.CheckFFmpeg())
            {
                InitializeComponents();
                Invalidate(true);
            }
            else
            {
                ffmpegCheckTimer.Start();
            }
        }

        private void SetDefaultButtonClicked(object sender, EventArgs e)
        {
            Content.Unbind();
            recordingOptions.FFmpegOptions = new FFmpegOptions();
            BindOptions();
        }

        private void BindOptions()
        {
            Content.DataContext = recordingOptions.FFmpegOptions;

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
            switch (recordingOptions.FFmpegOptions.VideoCodec)
            {
                case FFmpegVideoCodec.Libx264:
                case FFmpegVideoCodec.Libx265:
                    options = new H264H265CodecOptions(recordingOptions.FFmpegOptions);
                    break;

                case FFmpegVideoCodec.Libvpx_vp9:
                    options = new Vp9CodecOptions(recordingOptions.FFmpegOptions);
                    break;

                case FFmpegVideoCodec.Libxvid:
                    options = new MpegCodecOptions(recordingOptions.FFmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }

        private void AudioCodecOptionsButtonClicked(object sender, EventArgs e)
        {
            Dialog options;
            switch (recordingOptions.FFmpegOptions.AudioCodec)
            {
                case FFmpegAudioCodec.Libvoaacenc:
                    options = new AacOptions(recordingOptions.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libopus:
                    options = new OpusOptions(recordingOptions.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libvorbis:
                    options = new VorbisOptions(recordingOptions.FFmpegOptions);
                    break;

                case FFmpegAudioCodec.Libmp3lame:
                    options = new Mp3Options(recordingOptions.FFmpegOptions);
                    break;

                default:
                    return;
            }

            options.ShowModal(this);
        }
    }
}
