using System.Linq;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.DataTransfer.Ffmpeg.Encoding;

namespace RedShot.Infrastructure.RecordingRedShot.Views
{
    internal partial class RecordingOptionsView : Dialog
    {
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
    }
}
