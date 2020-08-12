using RedShot.Recording.Devices;

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
