using Eto.Drawing;
using RedShot.Configuration;
using RedShot.Helpers;
using RedShot.Recording.Forms;
using RedShot.Recording.Recorders;
using RedShot.Recording.Recorders.Linux;
using RedShot.Recording.Recorders.Windows;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RedShot.Recording
{
    public static class Recorder
    {
        public static void InitiateRecording()
        {
            IRecordingManager manager;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                manager = new LinuxRecordingManager();
            }
            else
            {
                manager = new WindowsRecordingManager();
            }

            if (!manager.CheckFFmpeg())
            {
                manager.InstallFFmpeg();
            }

            using (var optionsView = new RecordingOptionsView(manager))
            {
                optionsView.ShowModal();
            }

            var options = ConfigurationManager.YamlConfig.FFmpegOptions;

            var recorder = manager.GetRecorder(options);

            Rectangle area = default;

            if (options.UseGdigrab || options.VideoDevice == null)
            {
                Task.Delay(500).Wait();

                using var areaForm = new AreaSelectingView();

                areaForm.ShowModal();

                if (areaForm.DialogResult == Eto.Forms.DialogResult.Ok)
                {
                    area = (Rectangle)areaForm.SelectionRectangle;
                }
            }
            else
            {
                area = (Rectangle)ScreenHelper.GetMainWindowSize();
            }

            if (area != default)
            {
                var view = new RecordingView(recorder, area);
                view.Show();
            }
        }
    }
}
