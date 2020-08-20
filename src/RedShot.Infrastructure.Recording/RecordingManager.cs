using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Eto.Drawing;
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
