using RedShot.Helpers.Ffmpeg;
using RedShot.Helpers.Ffmpeg.Devices;

namespace RedShot.Recording.Recorders
{
    public interface IRecordingManager
    {
        IRecorder GetRecorder(FFmpegOptions options);

        void InstallFFmpeg();

        bool CheckFFmpeg();

        RecordingDevices GetRecordingDevices();
    }
}
