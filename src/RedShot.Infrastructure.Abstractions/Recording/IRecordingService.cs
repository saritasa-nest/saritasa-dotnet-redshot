namespace RedShot.Infrastructure.Abstractions.Recording
{
    public interface IRecordingService
    {
        IRecorder GetRecorder();

        bool InstallFFmpeg();

        bool CheckFFmpeg();

        IRecordingDevices GetRecordingDevices();
    }
}
