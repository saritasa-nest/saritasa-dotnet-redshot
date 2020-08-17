using System;
using System.Linq;
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
        private ComboBox audioCodec;

        private DefaultButton okButton;

        public RecordingOptionsView(IRecordingManager recordingManager)
        {
            recordingDevices = recordingManager.GetRecordingDevices();

            InitializeComponents();

            this.Closing += RecordingOptionsView_Closing;
            this.LoadComplete += RecordingOptionsView_LoadComplete;
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
                Size = new Size(250, 21),
            };
            videoCodec.DataStore = EnumDescription<FFmpegVideoCodec>.GetEnumDescriptions(typeof(FFmpegVideoCodec));

            audioCodec = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            audioCodec.DataStore = EnumDescription<FFmpegAudioCodec>.GetEnumDescriptions(typeof(FFmpegAudioCodec));

            okButton = new DefaultButton("OK", 70, 30);
            okButton.Clicked += OkButton_Clicked;

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = 20,
                Items =
                {
                    GetDeviceSelectionControl(),
                    FormsHelper.VoidBox(20),
                    GetEncodingSelectionControl(),
                    FormsHelper.VoidBox(10),
                    okButton
                }
            };

            BindOptions();
        }

        private Control GetMicrophoneControl()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 10,
                Items =
                {
                    useMicrophone,
                    FormsHelper.VoidBox(10),
                    new Label()
                    {
                        Text = "Use Microphone"
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

            return new GroupBox()
            {
                Text = "Devices",
                Content = new StackLayout()
                {
                    Orientation = Orientation.Vertical,
                    Padding = 20,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
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

            return new GroupBox()
            {
                Text = "Encoding",
                Content = new StackLayout()
                {
                    Orientation = Orientation.Vertical,
                    Padding = 20,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Items =
                    {
                        videoLabel,
                        FormsHelper.VoidBox(10),
                        videoCodec,
                        FormsHelper.VoidBox(20),
                        audioLabel,
                        FormsHelper.VoidBox(10),
                        audioCodec,
                        FormsHelper.VoidBox(25),
                    }
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
