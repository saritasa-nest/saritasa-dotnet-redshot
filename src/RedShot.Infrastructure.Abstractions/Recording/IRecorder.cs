using Eto.Drawing;

namespace RedShot.Infrastructure.Abstractions.Recording
{
    public interface IRecorder
    {
        void Start(Rectangle area);

        void Stop();

        IFile GetVideo();

        bool IsRecording { get; }
    }
}
