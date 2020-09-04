using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;
using RedShot.Infrastructure.Recording;
using RedShot.Infrastructure.Recording.Validation;
using RedShot.Recording.Settings.CodecsOptions.AudioOptions;
using RedShot.Recording.Settings.CodecsOptions.VideoOptions;

namespace RedShot.Infrastructure.Recording.Settings
{
    /// <summary>
    /// Recording option dialog.
    /// </summary>
    internal partial class RecordingOptionControl : Panel
    {
        private readonly FFmpegConfiguration ffmpegConfiguration;
        private readonly IRecordingDevices recordingDevices;
        private FFmpegOptions ffmpegOptions;
        private ComboBox videoDevices;
        private ComboBox primaryAudioDevices;
        private ComboBox optionalAudioDevices;
        private CheckBox useAudio;
        private ComboBox videoCodec;
        private DefaultButton videoCodecOptionsButton;
        private ComboBox audioCodec;
        private DefaultButton audioCodecOptionsButton;
        private DefaultButton setDefaultButton;
        private NumericStepper fps;
        private TextBox userArgs;
        private CheckBox showCursor;
        private CheckBox useGdigrab;

        /// <summary>
        /// Initializes recording option dialog.
        /// </summary>
        public RecordingOptionControl(IRecordingDevices recordingDevices, FFmpegConfiguration ffmpegConfiguration)
        {
            this.ffmpegConfiguration = ffmpegConfiguration;
            ffmpegOptions = ffmpegConfiguration.Options;
            this.recordingDevices = recordingDevices;

            InitializeComponents();
        }

        private void SetDefaultButton_Clicked(object sender, EventArgs e)
        {
            ffmpegOptions = new FFmpegOptions();
            Content.Unbind();
            BindOptions();
        }

        private void BindOptions()
        {
            Content.DataContext = ffmpegOptions;

            fps.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.Fps);
            userArgs.TextBinding.BindDataContext((FFmpegOptions o) => o.UserArgs);
            var gdigrabBind = useGdigrab.CheckedBinding.BindDataContext((FFmpegOptions o) => o.UseGdigrab);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                gdigrabBind.Changing += (o, e) =>
                {
                    if (e.Value is bool value)
                    {
                        videoDevices.Enabled = !value;
                    }
                };
            }

            showCursor.CheckedBinding.BindDataContext((FFmpegOptions o) => o.DrawCursor);

            useAudio.CheckedBinding.BindDataContext((FFmpegOptions o) => o.UseAudio);

            videoDevices.SelectedValueBinding.BindDataContext((FFmpegOptions o) => o.VideoDevice);

            primaryAudioDevices.SelectedValueBinding.BindDataContext((FFmpegOptions o) => o.PrimaryAudioDevice);
            primaryAudioDevices.Bind(d => d.Enabled, ffmpegOptions, o => o.UseAudio);

            optionalAudioDevices.SelectedValueBinding.BindDataContext((FFmpegOptions o) => o.OptionalAudioDevice);
            optionalAudioDevices.Bind(d => d.Enabled, ffmpegOptions, o => o.UseAudio);

            audioCodec.SelectedValueBinding.Convert(
                l =>
                {
                    if (l == null)
                    {
                        return FFmpegAudioCodec.libvoaacenc;
                    }
                    else
                    {
                        var des = (EnumDescription<FFmpegAudioCodec>)l;
                        return des.EnumValue;
                    }
                },
                v =>
                {
                    return audioCodec.DataStore.FirstOrDefault(o => ((EnumDescription<FFmpegAudioCodec>)o).EnumValue == v);
                }).BindDataContext((FFmpegOptions o) => o.AudioCodec);

            videoCodec.SelectedValueBinding.Convert(
                l =>
                {
                    if (l == null)
                    {
                        return FFmpegVideoCodec.libx264;
                    }
                    else
                    {
                        var des = (EnumDescription<FFmpegVideoCodec>)l;
                        return des.EnumValue;
                    }
                },
                v =>
                {
                    return videoCodec.DataStore.FirstOrDefault(o => ((EnumDescription<FFmpegVideoCodec>)o).EnumValue == v);
                }).BindDataContext((FFmpegOptions o) => o.VideoCodec);
        }

        private void VideoCodecOptionsButton_Clicked(object sender, EventArgs e)
        {
            switch (ffmpegOptions.VideoCodec)
            {
                case FFmpegVideoCodec.libx264:
                case FFmpegVideoCodec.libx265:
                    using (var options = new H264H265CodecOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegVideoCodec.libvpx_vp9:
                    using (var options = new Vp9CodecOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegVideoCodec.libxvid:
                    using (var options = new MpegCodecOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;
            }
        }

        private void AudioCodecOptionsButton_Clicked(object sender, EventArgs e)
        {
            switch (ffmpegOptions.AudioCodec)
            {
                case FFmpegAudioCodec.libvoaacenc:
                    using (var options = new AacOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.libopus:
                    using (var options = new OpusOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.libvorbis:
                    using (var options = new VorbisOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.libmp3lame:
                    using (var options = new Mp3Options(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;
            }
        }
    }
}
