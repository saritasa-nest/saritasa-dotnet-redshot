using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Recording.Recorders.Linux;
using RedShot.Recording.Recorders.Windows;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.RecordingRedShot.Views;

namespace RedShot.Infrastructure.Recording
{
    public static class RecordingManager
    {
        private static IRecordingService manager;
        private static RecordingView recordingView;

        static RecordingManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                manager = new LinuxRecordingService();
            }
            else
            {
                manager = new WindowsRecordingService();
            }
        }

        public static void OpenSettings()
        {
            using (var optionsView = new RecordingOptionsView(manager))
            {
                optionsView.ShowModal();
            }

            Task.Delay(500).Wait();
        }

        public static void RecordRegion(Rectangle region)
        {
            var recorder = manager.GetRecorder();

            recordingView = new RecordingView(recorder, region);
            recordingView.Show();
        }

        public static void OpenSelectionView()
        {
            RecordingRegionSelectionView.SelectionScreen = Screen.PrimaryScreen;
            var view = new RecordingRegionSelectionView();
            view.Show();
        }

        public static void InitiateRecording()
        {
            if (!manager.CheckFFmpeg())
            {
                if (!manager.InstallFFmpeg())
                {
                    return;
                }
            }

            ConfigureDevices();

            recordingView?.Close();

            var optionsView = new RecordingOptionsView(manager);

            if (optionsView.ShowModal(new Form()) == DialogResult.Ok)
            {
                Task.Delay(500).Wait();

                OpenSelectionView();
            }
        }

        /// <summary>
        /// Remove devices from FFmpeg configuration if there aren't such devices in the OS.
        /// </summary>
        private static void ConfigureDevices()
        {
            var configuration = ConfigurationManager.GetSection<FFmpegConfiguration>();
            var options = configuration.Options;

            var recordingDevices = manager.GetRecordingDevices();

            if (options.AudioDevice != null)
            {
                if (!recordingDevices.AudioDevices.Any(d => d.Name == options.AudioDevice.Name))
                {
                    options.AudioDevice = null;
                }
            }

            if (options.VideoDevice != null)
            {
                if (!recordingDevices.VideoDevices.Any(d => d.Name == options.VideoDevice.Name))
                {
                    options.VideoDevice = null;
                }
            }

            ConfigurationManager.SetSettingsValue(configuration);
        }
    }
}
