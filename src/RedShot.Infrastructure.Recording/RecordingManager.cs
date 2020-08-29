using System.Runtime.InteropServices;
using System.Linq;
using Eto.Drawing;
using RedShot.Infrastructure.Configuration;
using RedShot.Recording.Recorders.Linux;
using RedShot.Recording.Recorders.Windows;
using RedShot.Infrastructure.Abstractions.Recording;
using RedShot.Infrastructure.RecordingRedShot.Views;
using System;

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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                RecordingService = new LinuxRecordingService();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RecordingService = new WindowsRecordingService();
            }
            else
            {
                throw new NotSupportedException("Recording on this OS is not supported!");
            }
        }

        /// <summary>
        /// Records video by specified region.
        /// </summary>
        public static void RecordRegion(Rectangle region)
        {
            var recorder = RecordingService.GetRecorder();

            recordingView = new RecordingView(recorder, region);
            recordingView.Show();
        }

        /// <summary>
        /// Opens recording selection view.
        /// </summary>
        public static void OpenSelectionView()
        {
            var view = new RecordingRegionSelectionView();
            view.Show();
        }

        /// <summary>
        /// Checks FFmpeg binaries in the OS.
        /// If they don't exist, it tries to install them.
        /// </summary>
        /// <returns>True, if the binaries are installed and ready to work.</returns>
        public static bool CheckInstallFfmpeg()
        {
            if (!RecordingService.CheckFFmpeg())
            {
                return RecordingService.InstallFFmpeg();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Tries to start recording; checks FFmpeg binaries before starting recorder.
        /// </summary>
        public static void InitiateRecording()
        {
            if (!CheckInstallFfmpeg())
            {
                return;
            }

            ConfigureDevices();
            recordingView?.Close();
            OpenSelectionView();
        }

        /// <summary>
        /// Remove devices from FFmpeg configuration if there aren't such devices in the OS.
        /// </summary>
        private static void ConfigureDevices()
        {
            var configuration = ConfigurationManager.GetSection<FFmpegConfiguration>();
            var options = configuration.Options;

            var recordingDevices = RecordingService.GetRecordingDevices();

            if (options.PrimaryAudioDevice != null)
            {
                if (!recordingDevices.AudioDevices.Any(d => d.Name == options.PrimaryAudioDevice.Name))
                {
                    options.PrimaryAudioDevice = null;
                }
            }

            if (options.OptionalAudioDevice != null)
            {
                if (!recordingDevices.AudioDevices.Any(d => d.Name == options.OptionalAudioDevice.Name))
                {
                    options.OptionalAudioDevice = null;
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
