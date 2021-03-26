using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Infrastructure.Recording.Views;
using RedShot.Infrastructure.Recording.Common;
using RedShot.Infrastructure.Configuration.Models.Recording;
using RedShot.Infrastructure.Common;
using RedShot.Infrastructure.Recording.Common.Devices;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Recording manager.
    /// </summary>
    public class RecordingManager
    {
        /// <summary>
        /// Instance of the Recording manager.
        /// </summary>
        public static RecordingManager Instance = new RecordingManager();

        /// <summary>
        /// Recording service for the OS.
        /// </summary>
        public IRecordingService RecordingService { get; }

        /// <summary>
        /// Recording view.
        /// </summary>
        private RecordingView recordingView;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordingManager()
        {
            RecordingService = new RecordingService();
        }

        /// <summary>
        /// Records video by specified region.
        /// </summary>
        public void RecordRegion(Rectangle region)
        {
            if (region.Width % 2 != 0)
            {
                region.Width--;
            }

            if (region.Height % 2 != 0)
            {
                region.Height--;
            }

            recordingView = new RecordingView(RecordingService, region);
            recordingView.Show();
        }

        /// <summary>
        /// Try to start recording.
        /// Check the FFmpeg binaries before running recorder,
        /// if they don't exist, suggest installing them.
        /// </summary>
        public void InitiateRecording()
        {
            if (!RecordingService.CheckFFmpeg())
            {
                const string message = "FFmpeg is not installed. Do you want to automatically install it?";
                const string title = "FFmpeg Installing";
                var yesNoDialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxType.Warning);
                if (yesNoDialogResult == DialogResult.Yes)
                {
                    try
                    {
                        RecordingService.InstallFFmpeg();
                    }
                    catch
                    {
                        MessageBox.Show("An error occurred when FFmpeg was installing!");
                    }
                }

                return;
            }

            ConfigureDevices();
            recordingView?.Close();
            OpenSelectionView();
        }

        /// <summary>
        /// Open recording selection view.
        /// </summary>
        private void OpenSelectionView()
        {
            var view = new RecordingRegionSelectionView();
            view.Show();
        }

        /// <summary>
        /// Remove devices from FFmpeg configuration if there aren't such devices in the OS.
        /// </summary>
        private void ConfigureDevices()
        {
            var configuration = ConfigurationProvider.Instance.GetConfiguration<RecordingConfiguration>();

            var recordingDevices = RecordingService.GetRecordingDevices();

            var audioDevicesData = configuration.AudioData.Devices;
            var audioDevices = Mapping.Mapper.Map<IEnumerable<Device>>(audioDevicesData);

            audioDevices = audioDevices.Intersect(recordingDevices.AudioDevices);

            configuration.AudioData.Devices = Mapping.Mapper.Map<IList<DeviceData>>(audioDevices);

            if (configuration.FFmpegData.VideoDevice != null)
            {
                var videoDeviceData = configuration.FFmpegData.VideoDevice;
                var videoDevice = Mapping.Mapper.Map<Device>(videoDeviceData);

                if (recordingDevices.VideoDevices.All(d => d.Name != videoDevice.Name))
                {
                    configuration.FFmpegData.VideoDevice = null;
                }
            }

            ConfigurationProvider.Instance.SetConfiguration(configuration);
        }
    }
}
