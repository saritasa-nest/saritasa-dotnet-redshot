using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Eto.Drawing;
using RedShot.Configuration;
using RedShot.Recording.Views;
using RedShot.Recording.Recorders;
using RedShot.Recording.Recorders.Linux;
using RedShot.Recording.Recorders.Windows;

namespace RedShot.Recording
{
    public static class FFmpegRecorder
    {
        private static IRecordingManager manager;
        private static RecordingView recordingView;

        static FFmpegRecorder()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                manager = new LinuxRecordingManager();
            }
            else
            {
                manager = new WindowsRecordingManager();
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
            var options = ConfigurationManager.YamlConfig.FFmpegOptions;
            var recorder = manager.GetRecorder(options);

            recordingView = new RecordingView(recorder, region);
            recordingView.Show();
        }

        public static void OpenSelectionView()
        {
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

            recordingView?.Close();

            OpenSettings();

            OpenSelectionView();
        }
    }
}
