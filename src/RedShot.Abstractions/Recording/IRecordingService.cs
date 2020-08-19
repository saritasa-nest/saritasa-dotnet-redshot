namespace RedShot.Abstractions.Recording
{
    public interface IRecordingService
    {
        IRecorder GetRecorder();

        bool InstallFFmpeg();

        bool CheckFFmpeg();

        IRecordingDevices GetRecordingDevices();
    }
}
