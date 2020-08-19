namespace RedShot.Abstractions.Recording
{
    public interface IRecordingDevice
    {
        string Name { get; }

        string CompatibleFfmpegName { get; }
    }
}