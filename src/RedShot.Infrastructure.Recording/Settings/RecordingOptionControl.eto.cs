using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Common.Forms;

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
                Width = 250
            };

            showCursor = new CheckBox();

            useGdigrab = new CheckBox();

            videoCodecOptionsButton = new Button
            {
                Text = "Options"
            };
            videoCodecOptionsButton.Click += VideoCodecOptionsButtonClicked;

            audioCodecOptionsButton = new Button
            {
                Text = "Options"
            };
            audioCodecOptionsButton.Click += AudioCodecOptionsButtonClicked;

            videoCodec = new ComboBox()
            {
                Size = new Size(180, 19),
                ReadOnly = true
            };

            audioCodec = new ComboBox()
            {
                Size = new Size(180, 19),
                ReadOnly = true
            };

            setDefaultButton = new Button()
            {
                Text = "Default"
            };
            setDefaultButton.Click += SetDefaultButtonClicked;

            Content = new TableLayout()
            {
                Padding = new Padding(20, 20, 0, 0),
                Spacing = new Size(0, 20),
                Rows =
                {
                    GetVideoOptionsRow(),
                    GetMiscellaneousOptionsRow(),
                    GetAudioOptionsRow(),
                    GetArgumentsOptionsRow(),
                    TableLayout.AutoSized(setDefaultButton, new Padding(2, 0, 0, 0))
                }
            };

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                useGdigrab.Enabled = false;
                ffmpegConfiguration.Options.UseGdigrab = false;
            }

            BindOptions();
        }

        private Control GetVideoCodecField()
        {
            return new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Spacing = 5,
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
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Spacing = 5,
                Items =
                {
                    audioCodec,
                    audioCodecOptionsButton,
                }
            };
        }

        private TableRow GetVideoOptionsRow()
        {
            var fpsLabel = new Label()
            {
                Text = "FPS"
            };

            var videoLabel = new Label()
            {
                Text = "Video encoding"
            };

            return new TableLayout()
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow()
                    {
                        Cells =
                        {
                            fpsLabel,
                            videoLabel
                        },
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            TableLayout.AutoSized(fps, new Padding(3, 0, 0, 0)),
                            TableLayout.AutoSized(GetVideoCodecField(), new Padding(3, 0, 0, 0))
                        }
                    }
                }
            };
        }

        private TableRow GetMiscellaneousOptionsRow()
        {
            var recordCursorLabel = new Label()
            {
                Text = "Record cursor"
            };

            var useGdigrabLabel = new Label()
            {
                Text = "Use Gdigrab"
            };

            return new TableLayout()
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow()
                    {
                        Cells =
                        {
                            recordCursorLabel,
                            showCursor
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            useGdigrabLabel,
                            useGdigrab
                        }
                    }
                }
            };
        }

        private TableRow GetAudioOptionsRow()
        {
            var audioLabel = new Label()
            {
                Text = "Audio encoding"
            };

            return new StackLayout()
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    audioLabel,
                    TableLayout.AutoSized(GetAudioCodecField(), new Padding(3, 0, 0, 0)),
                }
            };
        }

        private TableRow GetArgumentsOptionsRow()
        {
            return new StackLayout
            {
                Orientation = Orientation.Vertical,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Spacing = 5,
                Items =
                {
                    new Label()
                    {
                        Text = "Custom FFmpeg arguments:"
                    },
                    TableLayout.AutoSized(userArgs, new Padding(3, 0, 0, 0))
                }
            };
        }
    }
}
