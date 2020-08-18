using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Configuration;
using RedShot.Helpers;
using RedShot.Helpers.Ffmpeg.Devices;
using RedShot.Helpers.Ffmpeg.Encoding;
using RedShot.Helpers.Ffmpeg.Options;
using RedShot.Helpers.Forms;
using RedShot.Recording.Recorders;
using RedShot.Recording.Views.CodecsOptions.AudioOptions;
using RedShot.Recording.Views.CodecsOptions.VideoOptions;

namespace RedShot.Recording.Views
{
    public class RecordingOptionsView : Dialog
    {
        private FFmpegOptions ffmpegOptions = ConfigurationManager.YamlConfig.FFmpegOptions.Clone();

        private RecordingDevices recordingDevices;

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

        public RecordingOptionsView(IRecordingManager recordingManager)
        {
            Title = "FFmpeg recording options";
            recordingDevices = recordingManager.GetRecordingDevices();

            InitializeComponents();

            this.Closing += RecordingOptionsView_Closing;
            this.LoadComplete += RecordingOptionsView_LoadComplete;

            ShowInTaskbar = true;
        }

        private void RecordingOptionsView_LoadComplete(object sender, EventArgs e)
        {
            Location = FormsHelper.GetCenterLocation(ClientSize);
        }

        private void RecordingOptionsView_Closing(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void InitializeComponents()
        {
            fps = new NumericStepper()
            {
                MinValue = 15,
                MaxValue = 60,
                Increment = 1
            };

            userArgs = new TextBox()
            {
                Size = new Size(250, 21)
            };

            showCursor = new CheckBox();

            useGdigrab = new CheckBox();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                useGdigrab.Enabled = false;
            }

            videoCodecOptionsButton = new DefaultButton("Options", 60, 25);

            videoCodecOptionsButton.Clicked += VideoCodecOptionsButton_Clicked;

            audioCodecOptionsButton = new DefaultButton("Options", 60, 25);

            audioCodecOptionsButton.Clicked += AudioCodecOptionsButton_Clicked;

            useMicrophone = new CheckBox();

            videoDevices = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            videoDevices.DataStore = recordingDevices.VideoDevices;

            if (videoDevices.DataStore.Count() == 0)
            {
                videoDevices.Enabled = false;
            }

            audioDevices = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            audioDevices.DataStore = recordingDevices.AudioDevices;

            if (audioDevices.DataStore.Count() == 0)
            {
                audioDevices.Enabled = false;
            }

            videoCodec = new ComboBox()
            {
                Size = new Size(180, 21),
            };
            videoCodec.DataStore = EnumDescription<FFmpegVideoCodec>.GetEnumDescriptions(typeof(FFmpegVideoCodec));

            audioCodec = new ComboBox()
            {
                Size = new Size(180, 21),
            };
            audioCodec.DataStore = EnumDescription<FFmpegAudioCodec>.GetEnumDescriptions(typeof(FFmpegAudioCodec));

            okButton = new DefaultButton("OK", 80, 30);
            okButton.Clicked += OkButton_Clicked;

            setDefaultButton = new DefaultButton("Set default", 100, 30);
            setDefaultButton.Clicked += SetDefaultButton_Clicked;

            var groupBoxSize = new Size(300, 230);

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 10,
                Items =
                {
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Items =
                        {
                            new GroupBox()
                            {
                                Text = "Default",
                                Size = new Size(250, 230),
                                Content = GetDefaultOptionControl(),
                            },
                            FormsHelper.VoidBox(15),
                            new GroupBox()
                            {
                                Text = "Encoding",
                                Size = groupBoxSize,
                                Content = GetEncodingSelectionControl(),
                            },
                            FormsHelper.VoidBox(15),
                            new GroupBox()
                            {
                                Text = "Recording devices",
                                Size = groupBoxSize,
                                Content = GetDeviceSelectionControl(),
                            }
                        }
                    },
                    FormsHelper.VoidBox(10),
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Items =
                        {
                            okButton,
                            FormsHelper.VoidBox(10),
                            setDefaultButton,
                            FormsHelper.VoidRectangle(40, 10),
                            new Label()
                            {
                                Text = "Custom FFmpeg arguments:"
                            },
                            FormsHelper.VoidBox(10),
                            userArgs
                        }
                    }
                }
            };

            BindOptions();
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

        private void SetDefaultButton_Clicked(object sender, EventArgs e)
        {
            ffmpegOptions = new FFmpegOptions();
            Content.Unbind();
            BindOptions();
        }


        private Control GetMicrophoneControl()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                {
                    useMicrophone,
                    FormsHelper.VoidBox(10),
                    new Label()
                    {
                        Text = "Use microphone"
                    }
                }
            };
        }

        private Control GetDeviceSelectionControl()
        {
            var videoLabel = new Label()
            {
                Text = "Video device"
            };

            var audioLabel = new Label()
            {
                Text = "Audio device"
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                    {
                        videoLabel,
                        FormsHelper.VoidBox(10),
                        videoDevices,
                        FormsHelper.VoidBox(20),
                        audioLabel,
                        FormsHelper.VoidBox(10),
                        audioDevices,
                        FormsHelper.VoidBox(15),
                        GetMicrophoneControl(),
                        FormsHelper.VoidBox(10),
                    }
            };
        }

        private Control GetEncodingSelectionControl()
        {
            var videoLabel = new Label()
            {
                Text = "Video encoding"
            };

            var audioLabel = new Label()
            {
                Text = "Audio encoding"
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                    {
                        videoLabel,
                        FormsHelper.VoidBox(10),
                        GetVideoCodecField(),
                        FormsHelper.VoidBox(20),
                        audioLabel,
                        FormsHelper.VoidBox(10),
                        GetAudioCodecField(),
                        FormsHelper.VoidBox(25),
                    }
            };
        }

        private Control GetVideoCodecField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    videoCodec,
                    FormsHelper.VoidBox(10),
                    videoCodecOptionsButton,
                }
            };
        }

        private Control GetAudioCodecField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Items =
                {
                    audioCodec,
                    FormsHelper.VoidBox(10),
                    audioCodecOptionsButton,
                }
            };
        }

        private Control GetDefaultOptionControl()
        {
            return new StackLayout()
            {
                Width = 300,
                Orientation = Orientation.Vertical,
                Padding = 20,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                    {
                        FormsHelper.VoidBox(10),
                        FormsHelper.GetBaseStack("FPS:", fps, 100, 100),
                        FormsHelper.VoidBox(15),
                        FormsHelper.GetBaseStack("Show cursor:", showCursor, 100, 100),
                        FormsHelper.VoidBox(15),
                        FormsHelper.GetBaseStack("Use Gdigrab:", useGdigrab, 100, 100),
                        FormsHelper.VoidBox(15),
                    }
            };
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
                ConfigurationManager.YamlConfig.FFmpegOptions = ffmpegOptions;
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
    }
}
