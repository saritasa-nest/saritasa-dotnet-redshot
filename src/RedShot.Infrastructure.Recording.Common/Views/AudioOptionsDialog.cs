using System;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Common.Devices;
using RedShot.Infrastructure.Recording.Common.Ffmpeg;

namespace RedShot.Infrastructure.Recording.Common.Views
{
    /// <summary>
    /// Recording settings dialog.
    /// </summary>
    internal class AudioOptionsDialog : Dialog
    {
        private readonly AudioOptions audioOptions;
        private readonly IRecordingService recordingService;
        private CheckBox recordAudio;
        private Button okButton;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioOptionsDialog(IRecordingService recordingService)
        {
            Title = "Recording Settings";
            this.recordingService = recordingService;
            audioOptions = UserConfiguration.Instance.GetOptionOrDefault<FFmpegConfigurationOption>().AudioOptions;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            recordAudio = new CheckBox()
            {
                Text = "Record audio"
            };
            recordAudio.CheckedBinding.Bind(audioOptions, o => o.RecordAudio);

            var tableLayout = new TableLayout()
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(0, 0, 10, 30),
                Rows =
                {
                    new TableRow(recordAudio)
                }
            };

            var audioDevices = recordingService.GetRecordingDevices().AudioDevices;
            foreach (var device in audioDevices)
            {
                AddAudioDeviceRow(tableLayout, device);
            }

            okButton = new Button(OkButtonClick)
            {
                Text = "OK",
                ToolTip = "OK",
                Width = 70
            };

            Content = new StackLayout()
            {
                Orientation = Orientation.Horizontal,
                MinimumSize = new Size(300, 200),
                Padding = 10,
                Items =
                {
                    tableLayout,
                    new StackLayoutItem(okButton, VerticalAlignment.Bottom)
                }
            };
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            SaveOptions();
            Close();
        }

        private void SaveOptions()
        {
            var configuration = UserConfiguration.Instance.GetOptionOrDefault<FFmpegConfigurationOption>();
            configuration.AudioOptions = audioOptions;

            UserConfiguration.Instance.SetOption(configuration);
            UserConfiguration.Instance.Save();
        }

        private void AddAudioDeviceRow(TableLayout tableLayout, Device audioDevice)
        {
            var deviceCheckbox = new CheckBox()
            {
                Text = audioDevice.Name
            };
            SetEnabledProperty(deviceCheckbox);
            recordAudio.CheckedChanged += (o, e) => SetEnabledProperty(deviceCheckbox);
            deviceCheckbox.Checked = audioOptions.Devices.Contains(audioDevice);
            deviceCheckbox.CheckedChanged += (o, e) =>
            {
                if (deviceCheckbox.Checked.GetValueOrDefault())
                {
                    audioOptions.Devices.Add(audioDevice);
                }
                else
                {
                    audioOptions.Devices.Remove(audioDevice);
                }
            };

            tableLayout.Rows.Add(deviceCheckbox);
        }

        private void SetEnabledProperty(CheckBox checkBox)
        {
            checkBox.Enabled = recordAudio.Checked.GetValueOrDefault();
        }
    }
}
