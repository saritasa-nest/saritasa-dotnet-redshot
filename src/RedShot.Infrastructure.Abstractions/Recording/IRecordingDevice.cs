namespace RedShot.Infrastructure.Abstractions.Recording
{
    public interface IRecordingDevice
    {
        string Name { get; }

        string CompatibleFfmpegName { get; }
    }
}