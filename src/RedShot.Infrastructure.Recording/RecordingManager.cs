using System.Runtime.InteropServices;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Configuration;
using RedShot.Recording.Recorders.Linux;
using RedShot.Recording.Recorders.Windows;
using RedShot.Infrastructure.RecordingRedShot.Views;
using RedShot.Infrastructure.Recording.Views;
using RedShot.Infrastructure.Recording.Recorders.MacOs;
using RedShot.Infrastructure.Recording.Abstractions;

namespace RedShot.Infrastructure.Recording
{
    /// <summary>
    /// Recording manager.
    /// </summary>
    public static class RecordingManager
    {
        /// <summary>
        /// Recording service for the OS.
        /// </summary>
        public static IRecordingService RecordingService { get; }

        /// <summary>
        /// Recording view.
        /// </summary>
        private static RecordingView recordingView;

        /// <summary>
        /// Initializes recording service depending on the OS.
        /// </summary>
        static RecordingManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                RecordingService = new MacOsRecordingService();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RecordingService = new WindowsRecordingService();
            }
            else
            {
                RecordingService = new LinuxRecordingService();
            }
        }

        /// <summary>
        /// Records video by specified region.
        /// </summary>
        public static void RecordRegion(Rectangle region)
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
        public static void InitiateRecording()
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
        private static void OpenSelectionView()
        {
            var view = new RecordingRegionSelectionView();
            view.Show();
        }

        /// <summary>
        /// Remove devices from FFmpeg configuration if there aren't such devices in the OS.
        /// </summary>
        private static void ConfigureDevices()
        {
            var configuration = UserConfiguration.Instance.GetOptionOrDefault<FFmpegConfiguration>();
            var options = configuration.FFmpegOptions;

            var recordingDevices = RecordingService.GetRecordingDevices();

            var usingDevices = configuration.AudioOptions.Devices.ToList();
            configuration.AudioOptions.Devices.Clear();
            configuration.AudioOptions.Devices.UnionWith(usingDevices.Intersect(recordingDevices.AudioDevices));

            if (options.VideoDevice != null)
            {
                if (recordingDevices.VideoDevices.All(d => d.Name != options.VideoDevice.Name))
                {
                    options.VideoDevice = null;
                }
            }

            UserConfiguration.Instance.SetOption(configuration);
            UserConfiguration.Instance.Save();
        }
    }
}
