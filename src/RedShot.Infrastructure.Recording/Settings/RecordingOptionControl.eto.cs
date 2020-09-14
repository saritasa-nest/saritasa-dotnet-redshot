using System.Linq;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Common.Forms;
using RedShot.Infrastructure.Recording.Ffmpeg.Encoding;

namespace RedShot.Infrastructure.Recording.Settings
{
    internal partial class RecordingOptionControl : Panel
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

            videoCodecOptionsButton = new DefaultButton("Options", 60, 25);

            videoCodecOptionsButton.Clicked += VideoCodecOptionsButton_Clicked;

            audioCodecOptionsButton = new DefaultButton("Options", 60, 25);

            audioCodecOptionsButton.Clicked += AudioCodecOptionsButton_Clicked;

            useAudio = new CheckBox();

            videoDevices = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            videoDevices.DataStore = recordingDevices.VideoDevices;

            if (videoDevices.DataStore.Count() == 0)
            {
                videoDevices.Enabled = false;
            }

            primaryAudioDevices = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            primaryAudioDevices.DataStore = recordingDevices.AudioDevices;

            if (primaryAudioDevices.DataStore.Count() == 0)
            {
                primaryAudioDevices.Enabled = false;
            }

            optionalAudioDevices = new ComboBox()
            {
                Size = new Size(250, 21),
            };
            optionalAudioDevices.DataStore = recordingDevices.AudioDevices;

            if (optionalAudioDevices.DataStore.Count() < 2)
            {
                optionalAudioDevices.Enabled = false;
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

            setDefaultButton = new DefaultButton("Set default", 100, 30);
            setDefaultButton.Clicked += SetDefaultButton_Clicked;

            var groupBoxSize = new Size(300, 230);

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Spacing = 15,
                        Items =
                        {
                            new GroupBox()
                            {
                                Text = "General",
                                Size = new Size(250, 230),
                                Content = GetDefaultOptionControl(),
                            },
                            new GroupBox()
                            {
                                Text = "Encoding",
                                Size = new Size(300, 230),
                                Content = GetEncodingSelectionControl(),
                            }
                        }
                    },
                    new GroupBox()
                    {
                        Text = "Recording devices",
                        Size = new Size(300, 250),
                        Content = GetDeviceSelectionControl()
                    },
                    new StackLayout()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Spacing = 10,
                        Items =
                        {
                            setDefaultButton,
                            FormsHelper.GetVoidRectangle(40, 10),
                            new Label()
                            {
                                Text = "Custom FFmpeg arguments:"
                            },
                            userArgs
                        }
                    }
                }
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (ffmpegOptions.UseGdigrab)
                {
                    videoDevices.Enabled = false;
                }
            }
            else
            {
                useGdigrab.Enabled = false;
                ffmpegOptions.UseGdigrab = false;
            }

            BindOptions();
        }

        private Control GetMicrophoneControl()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 10,
                Items =
                {
                    useAudio,
                    new Label()
                    {
                        Text = "Use audio"
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

            var primaryAudioLabel = new Label()
            {
                Text = "Primary audio device"
            };

            var optionalAudioLabel = new Label()
            {
                Text = "Optional audio device"
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 20,
                Spacing = 5,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Items =
                    {
                        videoLabel,
                        videoDevices,
                        FormsHelper.GetVoidBox(15),
                        primaryAudioLabel,
                        primaryAudioDevices,
                        optionalAudioLabel,
                        optionalAudioDevices,
                        FormsHelper.GetVoidBox(10),
                        GetMicrophoneControl()
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
                Spacing = 10,
                Items =
                    {
                        videoLabel,
                        GetVideoCodecField(),
                        FormsHelper.GetVoidBox(10),
                        audioLabel,
                        GetAudioCodecField()
                    }
            };
        }

        private Control GetVideoCodecField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    videoCodec,
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
                Spacing = 10,
                Items =
                {
                    audioCodec,
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
                Spacing = 15,
                Items =
                    {
                        FormsHelper.GetBaseStack("FPS:", fps, 100, 150),
                        FormsHelper.GetBaseStack("Show cursor:", showCursor, 100, 150),
                        FormsHelper.GetBaseStack("Use Gdigrab:", useGdigrab, 100, 150),
                    }
            };
        }
    }
}
