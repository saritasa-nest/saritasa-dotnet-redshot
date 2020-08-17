using RedShot.Helpers.Ffmpeg.Devices;
using RedShot.Helpers.Ffmpeg.Options;

namespace RedShot.Recording.Recorders
{
    public interface IRecordingManager
    {
        IRecorder GetRecorder(FFmpegOptions options);

        bool InstallFFmpeg();

        bool CheckFFmpeg();

        RecordingDevices GetRecordingDevices();
    }
}
