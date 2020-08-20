using System;
using System.Linq;
using System.Text;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Configuration.Options;
using RedShot.Infrastructure.DataTransfer.Ffmpeg;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Devices;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;
using RedShot.Recording.Views.CodecsOptions.AudioOptions;
using RedShot.Recording.Views.CodecsOptions.VideoOptions;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    internal partial class RecordingOptionsView : Dialog
    {
        private readonly FFmpegConfiguration ffmpegConfiguration;
        private readonly IRecordingDevices recordingDevices;
        private FFmpegOptions ffmpegOptions;
        private ComboBox videoDevices;
        private ComboBox audioDevices;
        private CheckBox useMicrophone;
        private ComboBox videoCodec;
        private DefaultButton videoCodecOptionsButton;
        private ComboBox audioCodec;
        private DefaultButton audioCodecOptionsButton;
        private DefaultButton okButton;
        private DefaultButton setDefaultButton;
        private NumericStepper fps;
        private TextBox userArgs;
        private CheckBox showCursor;
        private CheckBox useGdigrab;

        public RecordingOptionsView(IRecordingService recordingManager)
        {
            Title = "FFmpeg recording options";
            ffmpegConfiguration = ConfigurationManager.GetSection<FFmpegConfiguration>();
            ffmpegOptions = ffmpegConfiguration.Options.Clone();
            recordingDevices = recordingManager.GetRecordingDevices();

            InitializeComponents();

            this.LoadComplete += RecordingOptionsView_LoadComplete;

            ShowInTaskbar = true;
        }

        private void RecordingOptionsView_LoadComplete(object sender, EventArgs e)
        {
            Location = FormsHelper.GetCenterLocation(ClientSize);
        }

        private void SetDefaultButton_Clicked(object sender, EventArgs e)
        {
            ffmpegOptions = new FFmpegOptions();
            Content.Unbind();
            BindOptions();
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            var result = ffmpegOptions.Validate();

            if (!result.IsSuccess)
            {
                MessageBox.Show(new StringBuilder().AppendJoin("\n", result.Errors).ToString(), "Validation error", MessageBoxButtons.OK, MessageBoxType.Warning);
            }
            else
            {
                ffmpegConfiguration.Options = ffmpegOptions;
                ConfigurationManager.SetSettingsValue(ffmpegConfiguration);
                ConfigurationManager.Save();
                Close();
            }
        }

        private void BindOptions()
        {
            Content.DataContext = ffmpegOptions;

            fps.ValueBinding.Convert(f => (int)f, t => t).BindDataContext((FFmpegOptions o) => o.Fps);
            userArgs.TextBinding.BindDataContext((FFmpegOptions o) => o.UserArgs);
            useGdigrab.CheckedBinding.BindDataContext((FFmpegOptions o) => o.UseGdigrab);
            showCursor.CheckedBinding.BindDataContext((FFmpegOptions o) => o.DrawCursor);

            useMicrophone.CheckedBinding.BindDataContext((FFmpegOptions o) => o.UseMicrophone);

            videoDevices.SelectedValueBinding.BindDataContext((FFmpegOptions o) => o.VideoDevice);

            audioDevices.SelectedValueBinding.BindDataContext((FFmpegOptions o) => o.AudioDevice);
            audioDevices.Bind(d => d.Enabled, ffmpegOptions, o => o.UseMicrophone);

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
