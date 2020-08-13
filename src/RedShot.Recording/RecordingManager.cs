using Eto.Drawing;
using RedShot.Recording.Forms;
using RedShot.Recording.Recorders.Windows;

namespace RedShot.Recording
{
    public static class RecordingManager
    {
        public static void InitiateRecording()
        {
            var manager = new WindowsRecordingManager();

            if (!manager.CheckFFmpeg())
            {
                manager.InstallFFmpeg();
            }

            var recorder = manager.GetRecorder(new FFmpegOptions());

            using var areaForm = new AreaSelectingView();

            areaForm.ShowModal();

            if (areaForm.DialogResult == Eto.Forms.DialogResult.Ok)
            {
                var view = new RecordingView(recorder, (Rectangle)areaForm.SelectionRectangle);
                view.Show();
            }
        }
    }
}
