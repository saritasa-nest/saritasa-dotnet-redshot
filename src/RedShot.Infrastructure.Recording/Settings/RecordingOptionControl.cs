using System;
using System.Linq;
using System.Runtime.InteropServices;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
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
        public RecordingOptionControl(IRecordingDevices recordingDevices, FFmpegOptions ffmpegOptions)
        {
            this.ffmpegOptions = ffmpegOptions;
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
                        return FFmpegAudioCodec.Libvoaacenc;
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
                        return FFmpegVideoCodec.Libx264;
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
                case FFmpegVideoCodec.Libx264:
                case FFmpegVideoCodec.Libx265:
                    using (var options = new H264H265CodecOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegVideoCodec.Libvpx_vp9:
                    using (var options = new Vp9CodecOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegVideoCodec.Libxvid:
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
                case FFmpegAudioCodec.Libvoaacenc:
                    using (var options = new AacOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.Libopus:
                    using (var options = new OpusOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.Libvorbis:
                    using (var options = new VorbisOptions(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;

                case FFmpegAudioCodec.Libmp3lame:
                    using (var options = new Mp3Options(ffmpegOptions))
                    {
                        options.ShowModal(this);
                    }
                    break;
            }
        }
    }
}
